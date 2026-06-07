using System.Text.Json.Serialization;
using StarCorp.Travel.Api.Middleware;
using StarCorp.Travel.Application;
using StarCorp.Travel.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "StarCorp.Travel API v1"));
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program;
