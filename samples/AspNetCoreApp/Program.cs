using Leightweight.Multitenancy.Samples.AspNetCoreApp.Services;
using Leightweight.Multitenancy.Samples.AspNetCoreApp.Tenants;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton<IWebsiteService, WebsiteService>();

builder.Services
    .AddHttpContextAccessor()
    .AddTenant<Website>()
    .WithResolver<WebsiteResolver>();

builder.Services
    .AddControllers();

var app = builder.Build();

app.UseTenant<Website>();

app.MapControllers();

app.Run();
