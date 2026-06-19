using PersonalHub.Application.Features.Users.Common;

namespace PersonalHub.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> RegisterAsync(
        string email,
        string password);

    Task<string> LoginAsync(
    string email,
    string password);

    Task<List<UserDto>> GetUsersAsync();

    Task<string> CreateUserAsync(
       string email,
       string password,
       string role);

    Task<UserDto?> GetUserByIdAsync(
       string id);

    Task UpdateUserAsync(
    string id,
    string email,
    string role);

    Task DeleteUserAsync(
    string id);

    Task UpdateProfileAsync(string userId, string firstName, string lastName, string address, string phoneNumber);

    Task ChangePasswordAsync(
    string userId,
    string currentPassword,
    string newPassword);

    Task AssignRoleAsync(string userId, string role);
}