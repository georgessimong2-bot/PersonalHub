using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using MudBlazor.Services;
using PersonalHub.Web.Components;
using PersonalHub.Web.Configuration;
using PersonalHub.Web.HttpHandlers;
using PersonalHub.Web.Services;
using PersonalHub.Web.Services.Auth;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Configuration
builder.Configuration.AddUserSecrets<Program>();

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));
#endregion

#region Logging (Serilog)
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
#endregion

#region HTTP CLIENT (IMPORTANT - Must be registered BEFORE services that use it)
builder.Services.AddTransient<AuthHeaderHandler>();
builder.Services.AddTransient<LoggingHttpHandler>();

builder.Services.AddHttpClient("Api", (sp, client) =>
{
    var config = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
    client.BaseAddress = new Uri(config.BaseUrl);
})
.AddHttpMessageHandler<LoggingHttpHandler>()
.AddHttpMessageHandler<AuthHeaderHandler>();
#endregion

#region AUTH CORE SERVICES
builder.Services
    .AddAuthentication()
    .AddScheme<NoOpAuthenticationSchemeOptions, NoOpAuthenticationHandler>("NoOp", options => { });

builder.Services.AddSingleton<TokenStore>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
#endregion

#region APP SERVICES
builder.Services.AddScoped<AuthCheckService>();
builder.Services.AddScoped<NoteService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<GoalService>();
builder.Services.AddScoped<FundService>();
builder.Services.AddScoped<FundTypeService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<InvestmentStrategyService>();
builder.Services.AddScoped<BenchmarkService>();
builder.Services.AddScoped<SfdrClassificationService>();
builder.Services.AddScoped<ShareClassService>();
builder.Services.AddScoped<SubFundService>();
builder.Services.AddScoped<InstrumentTypeService>();
builder.Services.AddScoped<InstrumentService>();
builder.Services.AddScoped<InstrumentPriceService>();
builder.Services.AddScoped<PortfolioService>();
builder.Services.AddScoped<PortfolioHoldingService>();

#endregion

#region MUD BLAZOR
builder.Services.AddMudServices();
#endregion

#region BLAZOR SERVER
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<Microsoft.AspNetCore.Components.Server.CircuitOptions>(options =>
{
    options.DetailedErrors = true;
});
#endregion

var app = builder.Build();

#region PIPELINE

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

#endregion

app.Run();