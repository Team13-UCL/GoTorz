using GoTorz.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GoTorz.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using GoTorz.Components.Middleware;
using GoTorz.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddDbContextFactory<GoTorzContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoTorzContext") ?? throw new InvalidOperationException("Connection string 'GoTorzContext' not found.")));

builder.Services.AddDbContextFactory<AuthContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoTorzContext") ?? throw new InvalidOperationException("Connection string 'GoTorzContext' not found.")));


builder.Services.AddHttpClient();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://test.api.amadeus.com/") });

// login authentication and path ways if the login should fail, or we want to denide access to certain parts
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie( options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/accessdenied";
    });

builder.Services.AddScoped<UserService>();

builder.Services.AddAuthentication();

builder.Services.AddScoped<AmadeusAuthService>();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

// Konfigurer Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Logger til konsollen
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) // Logger til en fil pr. dag
    .CreateLogger();

// Tilføj Serilog som Logger
//builder.Host.UseSerilog();

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HttpClient
builder.Services.AddHttpClient();


//// Register the PackageService with the connection string
//builder.Services.AddSingleton(new PackageService(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register AmadeusAuthService
builder.Services.AddSingleton<AmadeusAuthService>(sp => {
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new AmadeusAuthService(httpClient, configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

// HTTP Authentication, it automatically checks if the user has a valid login-cookie
app.UseAuthentication();
// Controlles if the user has the rights to access to different pages
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseMiddleware<AuthMiddleware>();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


