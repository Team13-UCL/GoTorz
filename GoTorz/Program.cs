using GoTorz.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog; // For Serilog
using Microsoft.EntityFrameworkCore;
using GoTorz.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using GoTorz.Components.Middleware;
using GoTorz.Services;
using Microsoft.AspNetCore.Components.Authorization; // Nødvendig for CascadingAuthenticationState
using Microsoft.AspNetCore.ResponseCompression; // For SignalR/Compression
using GoTorz.Hubs;                      // For SignalR Hubs

var builder = WebApplication.CreateBuilder(args);

// --- Start på Serilog Konfiguration (Din Oprindelige Metode) ---
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Logger til konsollen
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) // Logger til en fil pr. dag
                                                                        // Tilføj evt. .ReadFrom.Configuration(builder.Configuration) her, hvis du OGSÅ vil læse fra appsettings.json
    .CreateLogger();

// Integrer Serilog med .NET logging systemet
builder.Host.UseSerilog();
// --- Slut på Serilog Konfiguration ---

// Konfigurer User Secrets
builder.Configuration.AddUserSecrets<Program>();

// ----- Add services to the container -----

// Database Contexts
builder.Services.AddDbContextFactory<GoTorzContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoTorzContext") ?? throw new InvalidOperationException("Connection string 'GoTorzContext' not found.")));
builder.Services.AddDbContextFactory<AuthContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoTorzContext") ?? throw new InvalidOperationException("Connection string 'GoTorzContext' not found.")));

// Scoped DbContexts
builder.Services.AddScoped(p => p.GetRequiredService<IDbContextFactory<GoTorzContext>>().CreateDbContext());
builder.Services.AddScoped(p => p.GetRequiredService<IDbContextFactory<AuthContext>>().CreateDbContext());

// HttpClient Factory
builder.Services.AddHttpClient();

// Authentication & Authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/accessdenied";
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState(); // Gør auth state tilgængelig

// Applikationsspecifikke Services
builder.Services.AddScoped<UserService>();
// Vælg én af disse til AmadeusAuthService (Singleton er ofte bedst pga. token caching)
// builder.Services.AddScoped<AmadeusAuthService>();
builder.Services.AddSingleton<AmadeusAuthService>(sp => {
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new AmadeusAuthService(httpClient, configuration);
});
builder.Services.AddScoped<HotelService>();

builder.Services.AddAuthentication();

builder.Services.AddScoped<AmadeusAuthService>();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

// Konfigurer Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Logger til konsollen
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) // Logger til en fil pr. dag
    .CreateLogger();

// Tilføj Serilog som Logger
builder.Host.UseSerilog();

builder.Services.AddQuickGridEntityFrameworkAdapter();

// Database Developer Page (kun for Development)
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Blazor Server Services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// SignalR Services
builder.Services.AddSignalR();

// Response Compression
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});


// ----- Build the App -----
var app = builder.Build();

// ----- Configure the HTTP request pipeline -----

// Response Compression (Placeres tidligt)
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// KORREKT RÆKKEFØLGE AF MIDDLEWARE
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.UseMiddleware<AuthMiddleware>(); // Din custom middleware

// KORREKT ENDPOINT MAPPING RÆKKEFØLGE
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<ChatHub>("/chathub");

// ----- Run the App -----
app.Run();


