namespace StarCorp.Travel.Application;
using Microsoft.Extensions.DependencyInjection;
using StarCorp.Travel.Application.Bookings.CancelBooking;
using StarCorp.Travel.Application.Bookings.CreateBooking;
using StarCorp.Travel.Application.Bookings.GetBooking;
using StarCorp.Travel.Application.Bookings.ProcessPayment;
using StarCorp.Travel.Application.Flights.SearchFlights;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<SearchFlightsHandler>();
        services.AddScoped<CreateBookingHandler>();
        services.AddScoped<GetBookingHandler>();
        services.AddScoped<ProcessPaymentHandler>();
        services.AddScoped<CancelBookingHandler>();

        return services;
    }
}
