using StarCorp.Travel.Application.Flights.SearchFlights;
using StarCorp.Travel.Application.Tests.Fakes;
using StarCorp.Travel.Domain.Flights;
using StarCorp.Travel.Domain.Shared;

namespace StarCorp.Travel.Application.Tests.Flights;

public class SearchFlightsHandlerTests
{
    private readonly FakeFlightRepository _flights = new();
    private readonly SearchFlightsHandler _handler;

    public SearchFlightsHandlerTests()
    {
        _handler = new SearchFlightsHandler(_flights);
    }

    private Flight NovoVoo(string origin, string destination, decimal price)
        => new(origin, destination, DateTime.UtcNow.AddDays(10), DateTime.UtcNow.AddDays(10).AddHours(2), price, 100, 20, Guid.NewGuid());

    [Fact]
    public async Task DeveRetornarVoosFiltradosPorOrigemEDestino()
    {
        _flights.Flights.Add(NovoVoo("São Paulo", "Rio de Janeiro", 500m));
        _flights.Flights.Add(NovoVoo("Brasília", "Salvador", 800m));

        var result = await _handler.HandleAsync(new SearchFlightsRequest(Origin: "São Paulo", Destination: "Rio de Janeiro"));

        Assert.Single(result.Items);
        Assert.Equal("São Paulo", result.Items[0].Origin);
    }

    [Fact]
    public async Task DeveFiltrarPorFaixaDePreco()
    {
        _flights.Flights.Add(NovoVoo("A", "B", 300m));
        _flights.Flights.Add(NovoVoo("A", "B", 700m));
        _flights.Flights.Add(NovoVoo("A", "B", 1200m));

        var result = await _handler.HandleAsync(new SearchFlightsRequest(MinPrice: 500m, MaxPrice: 1000m));

        Assert.Single(result.Items);
        Assert.Equal(700m, result.Items[0].BasePrice);
    }

    [Fact]
    public async Task DevePaginarResultados()
    {
        for (var i = 0; i < 15; i++)
            _flights.Flights.Add(NovoVoo("A", "B", 500m));

        var result = await _handler.HandleAsync(new SearchFlightsRequest(Page: 2, PageSize: 10));

        Assert.Equal(5, result.Items.Count);
        Assert.Equal(2, result.Page);
        Assert.Equal(15, result.TotalCount);
        Assert.Equal(2, result.TotalPages);
    }

    [Fact]
    public async Task DeveNormalizarPaginaInvalida()
    {
        _flights.Flights.Add(NovoVoo("A", "B", 500m));

        var result = await _handler.HandleAsync(new SearchFlightsRequest(Page: 0, PageSize: 0));

        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
    }

    [Fact]
    public async Task DeveFalharQuandoPrecoMinimoMaiorQueMaximo()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.HandleAsync(new SearchFlightsRequest(MinPrice: 1000m, MaxPrice: 100m)));
    }
}
