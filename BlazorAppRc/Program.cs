using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using BlazorAppRc.Data;
using BlazorAppRc.Services;

var builder = WebApplication.CreateBuilder(args);

//REGISTRO CORRECTO de HttpClient para consumir la API backend
builder.Services.AddScoped<HttpClient>(sp =>
{
    return new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7255") // tu API backend
    };
});

// Add services to the container.
builder.Services.AddScoped<AuthService>();
builder.Services.AddSingleton<AuthState>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
