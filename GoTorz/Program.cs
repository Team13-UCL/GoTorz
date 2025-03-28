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

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<GoTorzContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoTorzContext") ?? throw new InvalidOperationException("Connection string 'GoTorzContext' not found.")));
builder.Services.AddHttpClient();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://test.api.amadeus.com/") });

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

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HttpClient
builder.Services.AddHttpClient();

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

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


