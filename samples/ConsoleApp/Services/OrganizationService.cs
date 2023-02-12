using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leightweight.Multitenancy.Samples.ConsoleApp.Tenants;

namespace Leightweight.Multitenancy.Samples.ConsoleApp.Services;

internal class OrganizationService
{
    private readonly Dictionary<string, Organization> _organizations = new(StringComparer.OrdinalIgnoreCase);

    public async Task<Organization?> GetOrganization(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return default;

        await SimulateDelay();

        return _organizations.TryGetValue(name, out var organization)
            ? organization
            : _organizations[name] = new(name, Random.Shared.Next(2, 999));
    }

    public async Task<IEnumerable<Organization>> GetTopOrganizations()
    {
        await SimulateDelay();

        return _organizations
            .Values
            .OrderByDescending(o => o.Likes)
            .Take(3);
    }

    private static Task SimulateDelay()
        => Task.Delay(Random.Shared.Next(10, 50));
}
