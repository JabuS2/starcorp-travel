namespace StarCorp.Travel.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using StarCorp.Travel.Application.Common;
using StarCorp.Travel.Application.Flights.SearchFlights;

[ApiController]
[Route("api/flights")]
public sealed class FlightsController : ControllerBase
{
    private readonly SearchFlightsHandler _searchFlightsHandler;

    public FlightsController(SearchFlightsHandler searchFlightsHandler)
    {
        _searchFlightsHandler = searchFlightsHandler;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<FlightResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<FlightResponse>>> Search([FromQuery] SearchFlightsRequest request, CancellationToken cancellationToken)
    {
        var result = await _searchFlightsHandler.HandleAsync(request, cancellationToken);
        return Ok(result);
    }
}
