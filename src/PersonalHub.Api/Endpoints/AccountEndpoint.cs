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
            IWebHostEnvironment env,
            ILogger<Program> logger) =>
        {
            try
            {
                logger.LogInformation("=== PROFILE PICTURE UPLOAD STARTED ===");

                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                {
                    logger.LogWarning("Upload: User ID not found in claims");
                    return Results.Unauthorized();
                }

                logger.LogInformation("Upload: User ID = {UserId}", userId);

                var appUser = await userManager.FindByIdAsync(userId);

                if (appUser is null)
                {
                    logger.LogWarning("Upload: User not found. UserId: {UserId}", userId);
                    return Results.NotFound();
                }

                logger.LogInformation("Upload: User found");

                var file = request.Form.Files.FirstOrDefault();

                if (file is null || file.Length == 0)
                {
                    logger.LogWarning("Upload: No file provided. Files count: {Count}", request.Form.Files.Count);
                    return Results.BadRequest("No file uploaded.");
                }

                logger.LogInformation("Upload: File received. Name: {FileName}, Size: {Size}, ContentType: {ContentType}", 
                    file.FileName, file.Length, file.ContentType);

                var extension = Path.GetExtension(file.FileName);
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

                if (!allowedExtensions.Contains(extension.ToLowerInvariant()))
                {
                    logger.LogWarning("Upload: Invalid extension {Extension}", extension);
                    return Results.BadRequest("Invalid file type.");
                }

                var fileName = $"{userId}{extension}";

                var webRoot =
                     env.WebRootPath ??
                     Path.Combine(env.ContentRootPath, "wwwroot");

                Directory.CreateDirectory(webRoot);

                var folder = Path.Combine(webRoot, "profiles");
                Directory.CreateDirectory(folder);

                var path = Path.Combine(folder, fileName);

                logger.LogInformation("Upload: Saving to path {Path}", path);

                await using var stream = File.Create(path);
                await file.CopyToAsync(stream);

                logger.LogInformation("Upload: File saved successfully");

                if (!string.IsNullOrWhiteSpace(appUser.ProfilePictureUrl))
                {
                    var oldFile =
                        Path.Combine(
                            webRoot,
                            appUser.ProfilePictureUrl.TrimStart('/'));

                    if (File.Exists(oldFile))
                    {
                        try
                        {
                            File.Delete(oldFile);
                            logger.LogInformation("Upload: Old file deleted");
                        }
                        catch (Exception ex)
                        {
                            logger.LogWarning(ex, "Upload: Could not delete old file (may be in use). Will be cleaned up later.");
                        }
                    }
                }

                appUser.ProfilePictureUrl = $"/profiles/{fileName}";

                var updateResult = await userManager.UpdateAsync(appUser);

                if (!updateResult.Succeeded)
                {
                    logger.LogError("Upload: Failed to update user. Errors: {Errors}", 
                        string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                    return Results.Problem("Failed to update profile");
                }

                logger.LogInformation("Upload: User profile updated with new picture URL");

                var result = new
                {
                    Url = $"{request.Scheme}://{request.Host}{appUser.ProfilePictureUrl}"
                };

                logger.LogInformation("Upload: Success. URL: {Url}", result.Url);
                logger.LogInformation("=== PROFILE PICTURE UPLOAD COMPLETED ===");

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "=== PROFILE PICTURE UPLOAD FAILED WITH EXCEPTION ===");
                return Results.Problem("An error occurred during upload");
            }
        });
    }
}