using BakingSisters.Web.Components;
using BakingSisters.Web.Data;
using BakingSisters.Web.Services;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using ApiService = BakingSisters.Web.Services.ApiService;
using IApiService = BakingSisters.Web.Services.IApiService;
using ILoginService = BakingSisters.Web.Services.ILoginService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<ILoginService, LoginService>();
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
   /* .AddAdditionalAssemblies(typeof(BakingSisters.Web.Client._Imports).Assembly)*/;

app.Run();
