USE StarCorpTravel;
GO

DECLARE @now DATETIME2 = SYSUTCDATETIME();

DECLARE @airlineLatam UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
DECLARE @airlineGol   UNIQUEIDENTIFIER = '22222222-2222-2222-2222-222222222222';

DECLARE @customerAna  UNIQUEIDENTIFIER = '33333333-3333-3333-3333-333333333333';
DECLARE @customerBruno UNIQUEIDENTIFIER = '44444444-4444-4444-4444-444444444444';

DECLARE @flightSpRj UNIQUEIDENTIFIER = '55555555-5555-5555-5555-555555555555';
DECLARE @flightRjSp UNIQUEIDENTIFIER = '66666666-6666-6666-6666-666666666666';
DECLARE @flightBsbSsa UNIQUEIDENTIFIER = '77777777-7777-7777-7777-777777777777';

INSERT INTO dbo.Airlines (Id, Name, Code, Cnpj, IsActive, CreatedAt, UpdatedAt) VALUES
(@airlineLatam, N'LATAM Airlines', N'LA', N'11.222.333/0001-81', 1, @now, @now),
(@airlineGol,   N'GOL Linhas Aéreas', N'G3', N'33.444.555/0001-22', 1, @now, @now);

INSERT INTO dbo.Customers (Id, Name, Email, PhoneNumber, Address, DocumentNumber, IsActive, CreatedAt, UpdatedAt) VALUES
(@customerAna,   N'Ana Souza',   N'ana@example.com',   N'+5511999990001', N'Rua A, 100 - São Paulo/SP', N'472.993.038-09', 1, @now, @now),
(@customerBruno, N'Bruno Lima',  N'bruno@example.com', N'+5521999990002', N'Rua B, 200 - Rio de Janeiro/RJ', N'529.982.247-25', 1, @now, @now);

INSERT INTO dbo.Flights (Id, Origin, Destination, DepartureTime, ArrivalTime, BasePrice, EconomySeats, BusinessSeats, AirlineId, IsActive, CreatedAt, UpdatedAt) VALUES
(@flightSpRj,  N'São Paulo', N'Rio de Janeiro', DATEADD(DAY, 15, @now), DATEADD(HOUR, 1, DATEADD(DAY, 15, @now)), 500.00,  120, 20, @airlineLatam, 1, @now, @now),
(@flightRjSp,  N'Rio de Janeiro', N'São Paulo', DATEADD(DAY, 20, @now), DATEADD(HOUR, 1, DATEADD(DAY, 20, @now)), 480.00,  120, 20, @airlineGol,   1, @now, @now),
(@flightBsbSsa, N'Brasília', N'Salvador',       DATEADD(DAY, 30, @now), DATEADD(HOUR, 2, DATEADD(DAY, 30, @now)), 750.00,  150, 30, @airlineLatam, 1, @now, @now);
GO
