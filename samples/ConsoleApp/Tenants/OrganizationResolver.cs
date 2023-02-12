using System.Threading.Tasks;
using JetBrains.Annotations;
using Leightweight.Multitenancy.Samples.ConsoleApp.Services;

namespace Leightweight.Multitenancy.Samples.ConsoleApp.Tenants;

[UsedImplicitly]
internal class OrganizationResolver : ITenantResolver<Organization>
{
    private readonly OrganizationService _organizationService;
    private readonly OrganizationTenantContext _tenantContext;

    public OrganizationResolver(
        OrganizationService organizationService,
        OrganizationTenantContext tenantContext)
    {
        _organizationService = organizationService;
        _tenantContext = tenantContext;
    }

    public async ValueTask<Organization?> Resolve()
    {
        return await _organizationService.GetOrganization(_tenantContext.Name);
    }
}
