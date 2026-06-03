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


}