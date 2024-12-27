using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ReslifeFiveBackEnd.Context;
using ReslifeFiveBackEnd.Repository;
using ReslifeFiveBusinessLayer.Service;
using ReslifeFiveFrontEnd.Application.Authentication;
using ReslifeFiveFrontEnd.Components;
using ReslifeFiveFrontEnd.Application.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BlocktestConnection")),
    ServiceLifetime.Scoped);
builder.Services.AddScoped<ISamlService, SamlService>();
builder.Services.AddScoped<IGenRepository,GenRepository>();
builder.Services.AddScoped<IGenService, GenService>();
builder.Services.AddScoped<ITimeZoneService, TimeZoneService>();
builder.Services.AddScoped<ITargetUserService, TargetUserService>();
builder.Services.AddBlazorBootstrap();



builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = new PathString("/Login");
    options.LogoutPath = new PathString("/Logout");
    //these relate to how the blazor router works
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperUserPolicy", policy => policy.RequireRole("SuperUser"));
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin", "SuperUser"));
});
builder.Services.AddAuthorizationCore(); // For Blazor authorization
builder.Services.AddCascadingAuthenticationState();


var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<SamlMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
