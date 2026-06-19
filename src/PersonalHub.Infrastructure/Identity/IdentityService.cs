using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PersonalHub.Application.Common.Exceptions;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Users.Common;
using PersonalHub.Infrastructure.Auth;
using PersonalHub.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalHub.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityService(
        UserManager<AppUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        RoleManager<IdentityRole> roleManager,
        AppDbContext context)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _roleManager = roleManager;
    }

    // =========================
    // REGISTER
    // =========================
    public async Task<string> RegisterAsync(string email, string password)
    {
        var user = new AppUser
        {
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new BusinessException(
                string.Join(", ",
                    result.Errors.Select(x => x.Description)));
        }

        // IMPORTANT: rôle cohérent
        await EnsureRoleExists("USER");
        await _userManager.AddToRoleAsync(user, "USER");

        return user.Id;
    }

    // =========================
    // LOGIN + JWT
    // =========================
    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            throw new BusinessException("User not found");

        var validPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!validPassword)
            throw new BusinessException("Invalid credentials");

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        // IMPORTANT: rôle standard ASP.NET
        claims.AddRange(
            roles.Select(role =>
                new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // =========================
    // USERS LIST
    // =========================
    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        var result = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            result.Add(new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName,
                Role = roles.FirstOrDefault() ?? string.Empty
            });
        }

        return result;
    }

    // =========================
    // UPDATE PROFILE
    // =========================
    public async Task UpdateProfileAsync(
        string userId,
        string firstName,
        string lastName,
        string address,
        string phoneNumber)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new Exception("User not found");

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Address = address;
        user.PhoneNumber = phoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ",
                result.Errors.Select(e => e.Description)));
        }
    }

    // =========================
    // CREATE USER WITH ROLE
    // =========================
    public async Task<string> CreateUserAsync(string email, string password, string role)
    {
        await EnsureRoleExists(role);

        var user = new AppUser
        {
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ",
                result.Errors.Select(e => e.Description)));
        }

        await _userManager.AddToRoleAsync(user, role);

        return user.Id;
    }

    // =========================
    // GET BY ID
    // =========================
    public async Task<UserDto?> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            ProfilePictureUrl = user.ProfilePictureUrl,
            Role = roles.FirstOrDefault() ?? string.Empty
        };
    }

    // =========================
    // UPDATE USER ROLE
    // =========================
    public async Task UpdateUserAsync(string id, string email, string role)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            throw new Exception($"User '{id}' not found.");

        user.Email = email;
        user.UserName = email;

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            throw new Exception(string.Join(", ",
                updateResult.Errors.Select(x => x.Description)));
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

        await _userManager.RemoveFromRolesAsync(user, currentRoles);

        await EnsureRoleExists(role);

        await _userManager.AddToRoleAsync(user, role);
    }

    // =========================
    // DELETE USER
    // =========================
    public async Task DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            throw new Exception($"User '{id}' not found.");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ",
                result.Errors.Select(x => x.Description)));
        }
    }

    // =========================
    // HELPER
    // =========================
    private async Task EnsureRoleExists(string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(role));

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ",
                    result.Errors.Select(e => e.Description)));
            }
        }
    }

    public async Task ChangePasswordAsync(
    string userId,
    string currentPassword,
    string newPassword)
    {
        var user =
            await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new BusinessException("User not found.");

        var result =
            await _userManager.ChangePasswordAsync(
                user,
                currentPassword,
                newPassword);

        if (!result.Succeeded)
        {
            throw new BusinessException(
                string.Join(", ",
                    result.Errors.Select(x => x.Description)));
        }
    }

    public async Task AssignRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new BusinessException("User not found.");

        if (!await _userManager.IsInRoleAsync(user, role))
        {
            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
            {
                throw new BusinessException(
                    string.Join(", ",
                        result.Errors.Select(x => x.Description)));
            }
        }
    }
}
