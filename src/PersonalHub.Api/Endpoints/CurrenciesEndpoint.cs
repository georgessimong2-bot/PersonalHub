using MediatR;
using PersonalHub.Application.Features.Currency.CreateCurrency;
using PersonalHub.Application.Features.Currency.DeleteCurrency;
using PersonalHub.Application.Features.Currency.GetCurrencies;
using PersonalHub.Application.Features.Currency.GetCurrencyById;
using PersonalHub.Application.Features.Currency.UpdateCurrency;

namespace PersonalHub.Api.Endpoints;

public static class CurrencyEndpoints
{
    public static void MapCurrencyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/currencies");

        group.MapGet("/", async (
            IMediator mediator) =>
        {
            return Results.Ok(
                await mediator.Send(
                    new GetCurrenciesCommand()));
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            var currency =
                await mediator.Send(
                    new GetCurrencyByIdCommand(id));

            return currency is null
                ? Results.NotFound()
                : Results.Ok(currency);
        });

        group.MapPost("/", async (
            CreateCurrencyCommand command,
            IMediator mediator) =>
        {
            var id =
                await mediator.Send(command);

            return Results.Ok(id);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCurrencyCommand command,
            IMediator mediator) =>
        {
            command.Id = id;

            await mediator.Send(command);

            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator) =>
        {
            await mediator.Send(
                new DeleteCurrencyCommand(id));

            return Results.NoContent();
        });
    }
}