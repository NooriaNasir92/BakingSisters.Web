using BakingSisters.Api.Data;
using BakingSisters.Web.Components;
using BakingSisters.Web.Services;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using ApiService = BakingSisters.Web.Services.ApiService;
using IApiService = BakingSisters.Web.Services.IApiService;
using ILoginService = BakingSisters.Web.Services.ILoginService;
using IToastService = BakingSisters.Web.Services.IToastService;
using ToastService = BakingSisters.Web.Services.ToastService;
using ICartService = BakingSisters.Web.Services.ICartService;
using CartService = BakingSisters.Web.Services.CartService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();
// Use BakeryDbContext instead of ApplicationDbContext for unified data access
builder.Services.AddDbContext<BakeryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IToastService, ToastService>();
builder.Services.AddScoped<BakingSisters.Api.Services.Auth.IAuthService, BakingSisters.Api.Services.Auth.AuthService>();
builder.Services.AddScoped<ICartService, CartService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

// Ensure database exists and is up-to-date
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<BakeryDbContext>();
        // Just ensure the database exists
        context.Database.EnsureCreated();
        // Seed the database with initial users if running in development
        if (app.Environment.IsDevelopment())
        {
            await BakingSisters.Api.Data.Seeds.UserSeed.SeedUsersAsync(context);
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database.");
    }
}

await app.RunAsync();
