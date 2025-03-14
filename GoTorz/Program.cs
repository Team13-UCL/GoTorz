using GoTorz.Components;

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

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//// Register the PackageService with the connection string
//builder.Services.AddSingleton(new PackageService(builder.Configuration.GetConnectionString("DefaultConnection")));




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
