using Leightweight.Multitenancy.Samples.ConsoleApp;
using Leightweight.Multitenancy.Samples.ConsoleApp.Services;
using Leightweight.Multitenancy.Samples.ConsoleApp.Tenants;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services
    .AddSingleton<UserService>()
    .AddSingleton<OrganizationService>();

services
    .AddScoped<UserTenantContext>()
    .AddScoped<OrganizationTenantContext>();

services
    .AddTenant<User>()
    .WithResolver<UserResolver>();

services
    .AddTenant<Organization>()
    .WithResolver<OrganizationResolver>();

services
    .AddScoped<MainLoop>()
    .AddScoped<UserLoop>();

var provider = services.BuildServiceProvider();

await provider
    .GetRequiredService<MainLoop>()
    .Execute();
