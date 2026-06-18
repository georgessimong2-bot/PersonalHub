using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using PersonalHub.Api;
using PersonalHub.Api.Middlewares;
using PersonalHub.Application;
using PersonalHub.Application.Common.Behaviors;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Infrastructure;
using PersonalHub.Infrastructure.Auth;
using PersonalHub.Infrastructure.Data;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

#region USER SECRETS
builder.Configuration.AddUserSecrets<Program>();
#endregion

#region OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
#endregion

#region CORS
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()
    ?? new[] { "https://localhost:7001", "https://localhost:7002" };

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
#endregion

#region DATABASE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region APPLICATION
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));


builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IAppDbContext>(sp =>
    sp.GetRequiredService<AppDbContext>());

builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
#endregion

#region JWT
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

Console.WriteLine("JWT SECRET LENGTH = " +
    (jwtSettings.Secret?.Length ?? 0));

Console.WriteLine("JWT ISSUER = " +
    jwtSettings.Issuer);

Console.WriteLine("JWT AUDIENCE = " +
    jwtSettings.Audience);
#endregion

#region AUTH
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret)),

            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.NameIdentifier,

            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();
                Console.WriteLine($"JWT MESSAGE RECEIVED - Authorization header: {(string.IsNullOrEmpty(authHeader) ? "MISSING" : "PRESENT")}");
                if (!string.IsNullOrEmpty(authHeader))
                {
                    Console.WriteLine($"   Header length: {authHeader.Length}");
                }
                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"JWT AUTHENTICATION FAILED");
                Console.WriteLine($"   Exception: {context.Exception?.GetType().Name}");
                Console.WriteLine($"   Message: {context.Exception?.Message}");
                if (context.Exception is not null)
                {
                    Console.WriteLine($"   Stack trace (first 200 chars): {context.Exception.StackTrace?.Substring(0, Math.Min(200, context.Exception.StackTrace.Length))}");
                }
                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                var userId = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var email = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                Console.WriteLine($"JWT TOKEN VALIDATED");
                Console.WriteLine($"   UserId: {userId ?? "NOT FOUND"}");
                Console.WriteLine($"   Email: {email ?? "NOT FOUND"}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
#endregion

var app = builder.Build();

Console.WriteLine($"CONTENT ROOT = {app.Environment.ContentRootPath}");
Console.WriteLine($"WEB ROOT = {app.Environment.WebRootPath}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await db.Database.MigrateAsync();
}

await SeedRoles(app);

async Task SeedRoles(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = ["USER", "ADMIN"];

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

#region PIPELINE
app.UseMiddleware<ExceptionMiddleware>();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();


if (!app.Environment.IsDevelopment())
{
    // app.UseHttpsRedirection();
}
else
{
    app.UseHttpsRedirection();
}
var webRoot =
    Path.Combine(
        app.Environment.ContentRootPath,
        "wwwroot");

Directory.CreateDirectory(webRoot);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(webRoot),
    RequestPath = ""
});
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();



app.Run();
#endregion