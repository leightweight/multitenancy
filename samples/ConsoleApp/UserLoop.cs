using System;
using System.Threading.Tasks;
using Leightweight.Multitenancy.Samples.ConsoleApp.Services;
using Leightweight.Multitenancy.Samples.ConsoleApp.Tenants;

namespace Leightweight.Multitenancy.Samples.ConsoleApp;

internal class UserLoop
{
    private readonly User _user;
    private readonly ITenant<Organization> _organization;
    private readonly OrganizationService _organizationService;

    public UserLoop(
        ITenant<User> user,
        ITenant<Organization> organization,
        OrganizationService organizationService)
    {
        _user = user.Tenant!;
        _organization = organization;
        _organizationService = organizationService;
    }

    public async Task Execute()
    {
        Console.WriteLine();

        Console.WriteLine($"Hello, {_user.Name}!");
        Console.WriteLine();

        Console.WriteLine($"Your unique ID is {_user.Id:N}.");
        Console.WriteLine();

        if (_organization.Tenant is null)
        {
            var topOrganizations = await _organizationService.GetTopOrganizations();

            Console.Write("You currently don't work for any organizations. ");
            Console.WriteLine("These organizations are well liked:");

            foreach (var organization in topOrganizations)
                Console.WriteLine($"{organization.Name} ({organization.Likes} likes)");
        }
        else
        {
            Console.Write($"You currently work for {_organization.Tenant.Name}. ");
            Console.WriteLine($"They have {_organization.Tenant.Likes} likes!");
        }

        Console.WriteLine();
        Console.WriteLine("Have a good day!");
        Console.WriteLine();
        Console.WriteLine();
    }
}
