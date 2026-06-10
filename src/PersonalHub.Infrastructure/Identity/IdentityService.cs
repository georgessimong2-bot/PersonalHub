using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Users.Common;
using PersonalHub.Infrastructure.Auth;
using PersonalHub.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalHub.Infrastructure.Identity;

public class IdentityService
    : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppDbContext _context;

    public IdentityService(
    UserManager<AppUser> userManager,
    IOptions<JwtSettings> jwtSettings,
    RoleManager<IdentityRole> roleManager,
    AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtSettings = jwtSettings.Value;
        _context = context;
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
            new Claim("sub", user.Id),
            new Claim("email", user.Email!)
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(
            roles.Select(role =>
                new Claim("role", role)));

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

        Console.WriteLine("BEFORE:");
        Console.WriteLine($"FN={user.FirstName}, LN={user.LastName}");

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Address = address;
        user.PhoneNumber = phoneNumber;


        Console.WriteLine("AFTER:");
        Console.WriteLine($"FN={user.FirstName}");
        Console.WriteLine($"LN={user.LastName}");
        Console.WriteLine($"ADDRESS={user.Address}");
        Console.WriteLine($"PHONE={user.PhoneNumber}");
        var result = await _userManager.UpdateAsync(user);

        Console.WriteLine("UPDATED = " + result.Succeeded);

        if (!result.Succeeded)
        {
            foreach (var e in result.Errors)
                Console.WriteLine(e.Description);
        }
    }

    public async Task<string> CreateUserAsync(
       string email,
       string password,
       string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            var roleResult =
                await _roleManager.CreateAsync(
                    new IdentityRole(role));

            if (!roleResult.Succeeded)
            {
                throw new Exception(
                    string.Join(", ",
                        roleResult.Errors.Select(e => e.Description)));
            }
        }

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
                string.Join(", ",
                    result.Errors.Select(e => e.Description)));
        }

        var roleAssignmentResult =
            await _userManager.AddToRoleAsync(
                user,
                role);

        if (!roleAssignmentResult.Succeeded)
        {
            throw new Exception(
                string.Join(", ",
                    roleAssignmentResult.Errors.Select(e => e.Description)));
        }

        return user.Id;
    }

    public async Task<UserDto?> GetUserByIdAsync(
    string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return null;
        }

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

            Role = roles.FirstOrDefault() ?? string.Empty
        };
    }

    public async Task UpdateUserAsync(
    string id,
    string email,
    string role)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            throw new Exception($"User '{id}' not found.");
        }

        user.Email = email;
        user.UserName = email;

        var updateResult =
            await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            throw new Exception(
                string.Join(", ",
                    updateResult.Errors.Select(x => x.Description)));
        }

        var currentRoles =
            await _userManager.GetRolesAsync(user);

        await _userManager.RemoveFromRolesAsync(
            user,
            currentRoles);

        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(
                new IdentityRole(role));
        }

        await _userManager.AddToRoleAsync(
            user,
            role);
    }

    public async Task DeleteUserAsync(
    string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            throw new Exception($"User '{id}' not found.");
        }

        var result =
            await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception(
                string.Join(", ",
                    result.Errors.Select(x => x.Description)));
        }
    }
}