using System.Threading.Tasks;
using JetBrains.Annotations;
using Leightweight.Multitenancy.Samples.ConsoleApp.Services;

namespace Leightweight.Multitenancy.Samples.ConsoleApp.Tenants;

[UsedImplicitly]
internal class UserResolver : ITenantResolver<User>
{
    private readonly UserService _userService;
    private readonly UserTenantContext _tenantContext;

    public UserResolver(
        UserService userService,
        UserTenantContext tenantContext)
    {
        _userService = userService;
        _tenantContext = tenantContext;
    }

    public ValueTask<User?> Resolve()
    {
        return ValueTask.FromResult(_tenantContext.Name is null
            ? default
            : _userService.GetUser(_tenantContext.Name));
    }
}
