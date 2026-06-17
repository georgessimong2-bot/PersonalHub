using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonalHub.Application.Features.Account.ChangePassword;
using PersonalHub.Application.Features.Account.Common;
using PersonalHub.Application.Features.Account.UpdateProfile;
using PersonalHub.Application.Features.Users.Common;
using PersonalHub.Application.Features.Users.GetUserById;
using PersonalHub.Infrastructure.Identity;
using System.Security.Claims;

namespace PersonalHub.Api.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/account")
                       .RequireAuthorization();

        group.MapGet("/profile", async (
            ClaimsPrincipal user,
            IMediator mediator) =>
        {

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
                return Results.Unauthorized();

            var result = await mediator.Send(new GetUserByIdCommand(userId));

            return result is null
                ? Results.NotFound()
                : Results.Ok(result);
        });

        group.MapPut("/profile", async (
            UpdateProfileDto dto,
            ClaimsPrincipal user,
            IMediator mediator) =>
        {

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Results.Unauthorized();

            try
            {
                await mediator.Send(new UpdateProfileCommand
                {
                    UserId = userId,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Address = dto.Address,
                    PhoneNumber = dto.PhoneNumber
                });

                return Results.NoContent();
            }
            catch (FluentValidation.ValidationException ex)
            {
                return Results.BadRequest(new
                {
                    Errors = ex.Errors.Select(e => e.ErrorMessage)
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    title: "Error updating profile",
                    detail: ex.Message,
                    statusCode: 500);
            }
        });

        group.MapPut("/password", async (
            ChangePasswordDto dto,
            ClaimsPrincipal user,
            IMediator mediator) =>
        {
            var userId =
                user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Results.Unauthorized();

            await mediator.Send(
                new ChangePasswordCommand
                {
                    UserId = userId,
                    CurrentPassword = dto.CurrentPassword,
                    NewPassword = dto.NewPassword
                });

            return Results.NoContent();
        });

        group.MapPost("/profile-picture", async (
            HttpRequest request,
            ClaimsPrincipal user,
            UserManager<AppUser> userManager,
            IWebHostEnvironment env) =>
        {
            var userId =
                user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Results.Unauthorized();

            var appUser =
                await userManager.FindByIdAsync(userId);

            if (appUser is null)
                return Results.NotFound();

            var file =
                request.Form.Files.FirstOrDefault();

            if (file is null)
                return Results.BadRequest("No file uploaded.");

            var extension =
                Path.GetExtension(file.FileName);

            var fileName =
                $"{Guid.NewGuid()}{extension}";

            Console.WriteLine($"CONTENT ROOT = {env.ContentRootPath}");
            Console.WriteLine($"WEB ROOT = {env.WebRootPath}");

            var webRoot =
                 env.WebRootPath ??
                 Path.Combine(env.ContentRootPath, "wwwroot");

            Directory.CreateDirectory(webRoot);

            Console.WriteLine($"FINAL WEBROOT = {webRoot}");
            var folder =
                Path.Combine(webRoot, "profiles");

            Console.WriteLine($"FOLDER = {folder}");
            Directory.CreateDirectory(folder);

            var path =
                Path.Combine(folder, fileName);

            await using var stream =
                File.Create(path);

            await file.CopyToAsync(stream);

            if (!string.IsNullOrWhiteSpace(appUser.ProfilePictureUrl))
            {
                var oldFile =
                    Path.Combine(
                        webRoot,
                        appUser.ProfilePictureUrl.TrimStart('/'));

                if (File.Exists(oldFile))
                {
                    File.Delete(oldFile);
                }
            }

            appUser.ProfilePictureUrl =
                $"/profiles/{fileName}";

            await userManager.UpdateAsync(appUser);

            Console.WriteLine($"FILE SAVED = {path}");
            Console.WriteLine($"FILE EXISTS = {File.Exists(path)}");

            return Results.Ok(new
            {
                Url = $"{request.Scheme}://{request.Host}{appUser.ProfilePictureUrl}"
            });
        });
    }
}