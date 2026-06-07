# StarCorp.Travel

Sistema de reserva de passagens aéreas em **.NET 10**, construído com **Clean Architecture**, **TDD** e **Dapper** (sem ORM) sobre **SQL Server**.

## Arquitetura

O projeto é dividido em camadas isoladas por dependência (a dependência aponta sempre para dentro, em direção ao domínio):

```
src/
├── StarCorp.Travel.Domain/          Entidades, regras de negócio e serviços de domínio (sem dependências externas)
├── StarCorp.Travel.Application/      Casos de uso, DTOs, interfaces de repositório
├── StarCorp.Travel.Infrastructure/  Repositórios Dapper, fábrica de conexão, relógio do sistema
└── StarCorp.Travel.Api/             Controllers, middleware de erros, Swagger, composição (DI)
tests/
├── StarCorp.Travel.Domain.Tests/      (xUnit)
└── StarCorp.Travel.Application.Tests/ (xUnit)
```

- **Domain**: `Customer`, `Airline`, `Flight`, `Passenger`, `Payment`, `Booking` (aggregate root) e os serviços de domínio `PricingService` e `CancellationService`. Validações centralizadas no `Guard`.
- **Application**: um *handler* por caso de uso (`SearchFlights`, `CreateBooking`, `GetBooking`, `ProcessPayment`, `CancelBooking`), com requests/responses dedicados e exceções de aplicação (`NotFoundException`, `ConflictException`, `BusinessRuleException`).
- **Infrastructure**: repositórios com Dapper e SQL parametrizado, paginação via `OFFSET/FETCH`.
- **Api**: REST + Swagger, middleware global de tratamento de erros mapeando exceções para códigos HTTP.

## Como rodar

### Pré-requisitos
- .NET SDK 10
- SQL Server (local, Express ou container)

### 1. Banco de dados

Os scripts ficam em `db/`. Execute na ordem (via `sqlcmd`, Azure Data Studio ou SSMS):

```bash
sqlcmd -S localhost -i db/schema.sql   # cria o banco StarCorpTravel e as tabelas
sqlcmd -S localhost -i db/seed.sql      # popula companhias, clientes e voos de exemplo
```

> O `schema.sql` recria as tabelas (drop/create). Use apenas em ambiente de desenvolvimento.

Ajuste a connection string em `src/StarCorp.Travel.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "SqlServer": "Server=localhost;Database=StarCorpTravel;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 2. Aplicação

```bash
dotnet run --project src/StarCorp.Travel.Api
```

A documentação interativa (Swagger UI) fica em `/swagger` e o documento OpenAPI em `/openapi/v1.json` (ambiente de desenvolvimento).

### 3. Testes

```bash
dotnet test
```

Atualmente **109 testes** (89 de domínio + 20 de aplicação), todos verdes.

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| GET  | `/api/flights` | Busca de voos com filtros (`origin`, `destination`, `date`, `minPrice`, `maxPrice`, `bookingClass`) e paginação (`page`, `pageSize`) |
| POST | `/api/bookings` | Cria reserva; retorna o *breakdown* do cálculo de preço |
| GET  | `/api/bookings/{id}` | Consulta reserva |
| POST | `/api/bookings/{id}/payment` | Processa pagamento e confirma a reserva |
| POST | `/api/bookings/{id}/cancel` | Cancela a reserva e calcula o reembolso |

Códigos HTTP: `400` (entrada inválida), `404` (não encontrado), `409` (conflito de estado, ex.: sem assentos / já paga / já cancelada), `422` (violação de regra de negócio).

## Regras de negócio

### Cálculo de preço (`PricingService`)
1. **Subtotal** = preço base × nº de passageiros × multiplicador da classe (Economy = `1.0`, Business = `2.5`)
2. **Impostos** = 8% do subtotal + R$ 45,00 por passageiro
3. **Taxa de serviço** = 5% sobre (subtotal + impostos)
4. **Ajuste por método de pagamento** sobre o total: Cartão de Crédito `+3%`, Pix `-5%`, Boleto `+1%`

O *breakdown* completo (subtotal, impostos, taxa, ajuste e total) é retornado na criação da reserva.

### Reembolso (`CancellationService`)
| Antecedência | Economy | Business |
|--------------|---------|----------|
| > 7 dias     | 100%    | 100%     |
| 2 a 7 dias   | 50%     | 75%      |
| < 2 dias     | 0%      | 25%      |

**Regra especial:** cancelamento em até 24h após o pagamento → reembolso de 100%, independentemente da antecedência.

## Decisões técnicas

- **Sem ORM (Dapper):** SQL explícito e parametrizado, com mapeamento manual via fábricas de reconstituição (`Entity.Restore(...)`). Isso mantém o domínio rico (propriedades imutáveis, invariantes no construtor) e ao mesmo tempo permite reidratar entidades do banco preservando `Id`, *timestamps* e estado — sem refletir sobre membros privados nem expor *setters* públicos.
- **Enums como multiplicadores/contadores:** assentos são contadores por classe (`EconomySeats`/`BusinessSeats`), não assentos individuais; `AirlineId` é um `Guid` no `Flight` (não o objeto completo).
- **Serviços de domínio puros e estáticos:** `PricingService` e `CancellationService` não têm efeitos colaterais e recebem o "agora" como parâmetro, o que os torna trivialmente testáveis e determinísticos.
- **`IDateTimeProvider`:** o tempo é injetado, permitindo testar pagamento/cancelamento de forma determinística.
- **Tratamento de erros centralizado:** um único middleware traduz exceções para `ProblemDetails` com o status correto, mantendo os *handlers* livres de preocupações de HTTP.
- **Handlers por caso de uso:** sem mediador externo, mantendo o fluxo explícito e de fácil leitura. As dependências entram por construtor (SOLID).
- **Validação de documentos:** CPF (passageiros/clientes) e CNPJ validados no `Guard`; CNPJ armazenado e exibido com máscara.
- **Multiplicador de classe:** o desafio especificou os percentuais de imposto/taxa/pagamento, mas não os multiplicadores de classe. Adotei `Economy = 1.0` e `Business = 2.5`, isolados como constantes em `PricingService` para fácil ajuste.

## O que eu faria diferente com mais tempo

- **Baixa de assentos transacional:** hoje a criação de reserva valida a disponibilidade, mas não decrementa o contador de assentos do voo. Com mais tempo eu faria a reserva + baixa de assentos numa única transação com **concorrência otimista** (coluna `RowVersion`) para evitar *overbooking* sob concorrência.
- **Testes de integração de repositório:** com Testcontainers (SQL Server) para exercitar o SQL real, hoje coberto apenas indiretamente.
- **Estado do pagamento no cancelamento:** registrar explicitamente o reembolso (status `Refunded` no `Payment`) e um histórico de transações, em vez de apenas calcular o valor.
- **Idempotência e `Unit of Work`:** chave de idempotência no pagamento e um *unit of work* compartilhando a conexão/transação entre repositórios em operações compostas.
- **Validação de entrada na borda:** FluentValidation nos requests da API para respostas `400` mais ricas, complementando os *guards* de domínio.
- **Observabilidade:** logging estruturado, correlação de requisições e métricas.
- **Autenticação/Autorização:** hoje a API é aberta; um cenário real exigiria identidade do cliente autenticado em vez de `CustomerId` no corpo.
```
