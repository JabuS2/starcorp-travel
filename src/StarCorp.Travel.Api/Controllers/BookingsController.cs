namespace StarCorp.Travel.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using StarCorp.Travel.Application.Bookings;
using StarCorp.Travel.Application.Bookings.CancelBooking;
using StarCorp.Travel.Application.Bookings.CreateBooking;
using StarCorp.Travel.Application.Bookings.GetBooking;
using StarCorp.Travel.Application.Bookings.ProcessPayment;

[ApiController]
[Route("api/bookings")]
public sealed class BookingsController : ControllerBase
{
    private readonly CreateBookingHandler _createBookingHandler;
    private readonly GetBookingHandler _getBookingHandler;
    private readonly ProcessPaymentHandler _processPaymentHandler;
    private readonly CancelBookingHandler _cancelBookingHandler;

    public BookingsController(
        CreateBookingHandler createBookingHandler,
        GetBookingHandler getBookingHandler,
        ProcessPaymentHandler processPaymentHandler,
        CancelBookingHandler cancelBookingHandler)
    {
        _createBookingHandler = createBookingHandler;
        _getBookingHandler = getBookingHandler;
        _processPaymentHandler = processPaymentHandler;
        _cancelBookingHandler = cancelBookingHandler;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateBookingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreateBookingResponse>> Create([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var response = await _createBookingHandler.HandleAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _getBookingHandler.HandleAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{id:guid}/payment")]
    [ProducesResponseType(typeof(ProcessPaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProcessPaymentResponse>> ProcessPayment(Guid id, [FromBody] ProcessPaymentRequest request, CancellationToken cancellationToken)
    {
        var response = await _processPaymentHandler.HandleAsync(id, request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(CancelBookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CancelBookingResponse>> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var response = await _cancelBookingHandler.HandleAsync(id, cancellationToken);
        return Ok(response);
    }
}
