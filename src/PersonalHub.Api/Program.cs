using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Api.Middlewares;
using PersonalHub.Application;
using PersonalHub.Application.Common.Behaviors;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Notes.CreateNote;
using PersonalHub.Application.Features.Notes.GetNoteById;
using PersonalHub.Application.Features.Notes.GetNotes;
using PersonalHub.Infrastructure.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(AssemblyReference).Assembly));

builder.Services.AddScoped<IAppDbContext>(
    provider => provider.GetRequiredService<AppDbContext>());

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssembly(
    typeof(AssemblyReference).Assembly);

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapPost("/api/notes",
    async (
        CreateNoteCommand command,
        IMediator mediator) =>
    {
        var id = await mediator.Send(command);

        return Results.Ok(id);
    });

app.MapGet("/api/notes",
    async (IMediator mediator) =>
    {
        var notes = await mediator.Send(
            new GetNotesCommand());

        return Results.Ok(notes);
    });

app.MapGet("/api/notes/{id:guid}",
    async (Guid id, IMediator mediator) =>
    {
        var note = await mediator.Send(
            new GetNoteByIdCommand(id));

        return note is null
            ? Results.NotFound()
            : Results.Ok(note);
    });

app.Run();


