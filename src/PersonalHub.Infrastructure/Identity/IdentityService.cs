using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Users.Common;
using PersonalHub.Infrastructure.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalHub.Infrastructure.Identity;

public class IdentityService
    : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public IdentityService(
        UserManager<AppUser> userManager,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;

        _jwtSettings = jwtSettings.Value;
    }
    public async Task<string> RegisterAsync(
        string email,
        string password)
    {
        var user = new AppUser
        {
            UserName = email,
            Email = email
        };

        var result =
            await _userManager.CreateAsync(
                user,
                password);


        if (!result.Succeeded)
        {
            throw new Exception(
                string.Join(
                    ", ",
                    result.Errors.Select(x => x.Description)));
        }

        await _userManager.AddToRoleAsync(user, "User");

        return user.Id;
    }
    public async Task<string> LoginAsync(
    string email,
    string password)
    {
        var user =
            await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            throw new Exception("Invalid credentials");
        }

        var validPassword =
            await _userManager.CheckPasswordAsync(
                user,
                password);

        if (!validPassword)
        {
            throw new Exception("Invalid credentials");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.Email!)
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(
            roles.Select(role =>
                new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                _jwtSettings.ExpirationInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        return await _userManager.Users
            .Select(x => new UserDto
            {
                Id = x.Id,
                Email = x.Email!
            })
            .ToListAsync();
    }
}