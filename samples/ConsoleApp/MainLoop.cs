using System;
using System.Threading.Tasks;
using Leightweight.Multitenancy.Samples.ConsoleApp.Tenants;
using Microsoft.Extensions.DependencyInjection;

namespace Leightweight.Multitenancy.Samples.ConsoleApp;

internal class MainLoop
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MainLoop(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task Execute()
    {
        while (true)
        {
            using var scope = _scopeFactory.CreateScope();

            PopulateUserName(scope.ServiceProvider.GetRequiredService<UserTenantContext>());
            PopulateOrganizationName(scope.ServiceProvider.GetRequiredService<OrganizationTenantContext>());

            await MultitenancyUtilities.PopulateTenant<User>(scope.ServiceProvider);
            await MultitenancyUtilities.PopulateTenant<Organization>(scope.ServiceProvider);

            await scope.ServiceProvider
                .GetRequiredService<UserLoop>()
                .Execute();
        }
    }

    private static void PopulateUserName(UserTenantContext context)
    {
        while (string.IsNullOrWhiteSpace(context.Name))
        {
            Console.Write("What is your name? ");
            context.Name = Console.ReadLine();
        }
    }

    private static void PopulateOrganizationName(OrganizationTenantContext context)
    {
        Console.Write("What organization do you work for? ");
        context.Name = Console.ReadLine();
    }
}
