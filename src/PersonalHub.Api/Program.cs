using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Api.Middlewares;
using PersonalHub.Application;
using PersonalHub.Application.Common.Behaviors;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Notes.Common;
using PersonalHub.Application.Features.Notes.CreateNote;
using PersonalHub.Application.Features.Notes.DeleteNote;
using PersonalHub.Application.Features.Notes.GetNoteById;
using PersonalHub.Application.Features.Notes.GetNotes;
using PersonalHub.Application.Features.Notes.UpdateNote;
using PersonalHub.Infrastructure.Data;
using PersonalHub.Infrastructure;
using PersonalHub.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PersonalHub.Infrastructure.Auth;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
    ?? new[] { "https://localhost:7001", "https://localhost:7002" };

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()));

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

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings =
    builder.Configuration
        .GetSection("JwtSettings")
        .Get<JwtSettings>()!;

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings.Secret))
            };
    });

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddAuthorization();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.UseCors();


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

app.MapPut("/api/notes/{id:guid}",
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

app.MapDelete("/api/notes/{id:guid}",
    async (
        Guid id,
        IMediator mediator) =>
    {
        await mediator.Send(
            new DeleteNoteCommand(id));

        return Results.NoContent();
    });

app.Run();


