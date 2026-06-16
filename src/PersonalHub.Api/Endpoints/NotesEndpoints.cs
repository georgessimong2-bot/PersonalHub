using MediatR;
using PersonalHub.Application.Features.Notes.CreateNote;
using PersonalHub.Application.Features.Notes.DeleteNote;
using PersonalHub.Application.Features.Notes.GetNoteById;
using PersonalHub.Application.Features.Notes.GetNotes;
using PersonalHub.Application.Features.Notes.UpdateNote;

namespace PersonalHub.Api.Endpoints;

public static class NotesEndpoints
{
    public static void MapNotesEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/notes")
            .RequireAuthorization();

        // GET ALL

        group.MapGet("",
            async (
                IMediator mediator) =>
            {
                var result =
                    await mediator.Send(
                        new GetNotesCommand());

                return Results.Ok(result);
            });

        // GET BY ID

        group.MapGet("{id:guid}",
            async (
                Guid id,
                IMediator mediator) =>
            {
                var result =
                    await mediator.Send(
                        new GetNoteByIdCommand(id));

                return Results.Ok(result);
            });

        // CREATE

        group.MapPost("",
            async (
                CreateNoteCommand command,
                IMediator mediator) =>
            {
                var result =
                    await mediator.Send(command);

                return Results.Ok(result);
            });

        // UPDATE

        group.MapPut("{id:guid}",
            async (
                Guid id,
                UpdateNoteCommand command,
                IMediator mediator) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest();
                }

                await mediator.Send(command);

                return Results.NoContent();
            });

        // DELETE

        group.MapDelete("{id:guid}",
            async (
                Guid id,
                IMediator mediator) =>
            {
                await mediator.Send(
                    new DeleteNoteCommand(id));

                return Results.NoContent();
            });


    }
}