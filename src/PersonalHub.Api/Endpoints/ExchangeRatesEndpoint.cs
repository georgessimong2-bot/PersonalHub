using MediatR;
using PersonalHub.Application.Features.ExchangeRates.CreateExchangeRate;
using PersonalHub.Application.Features.ExchangeRates.DeleteExchangeRate;
using PersonalHub.Application.Features.ExchangeRates.GetExchangeRateById;
using PersonalHub.Application.Features.ExchangeRates.GetExchangeRates;
using PersonalHub.Application.Features.ExchangeRates.UpdateExchangeRate;

namespace PersonalHub.Api.Endpoints;

public static class ExchangeRatesEndpoint
{
    public static void MapExchangeRatesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/exchangerates");

        group.MapGet("/", async (
            Guid? fromCurrencyId,
            Guid? toCurrencyId,
            DateTime? dateFrom,
            DateTime? dateTo,
            IMediator mediator) =>
        {
            return Results.Ok(
                await mediator.Send(
                    new GetExchangeRatesQuery(
                        fromCurrencyId,
                        toCurrencyId,
                        dateFrom,
                        dateTo)));
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            var exchangeRate =
                await mediator.Send(
                    new GetExchangeRateByIdQuery(id));

            return exchangeRate is null
                ? Results.NotFound()
                : Results.Ok(exchangeRate);
        });

        group.MapPost("/", async (
            CreateExchangeRateCommand command,
            IMediator mediator) =>
        {
            var id =
                await mediator.Send(command);

            return Results.Ok(id);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateExchangeRateCommand command,
            IMediator mediator) =>
        {
            await mediator.Send(command);

            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(
                new DeleteExchangeRateCommand(id));

            return Results.NoContent();
        });
    }
}
