using System.Threading.Tasks;
using JetBrains.Annotations;
using Leightweight.Multitenancy.Samples.AspNetCoreApp.Services;
using Microsoft.AspNetCore.Http;

namespace Leightweight.Multitenancy.Samples.AspNetCoreApp.Tenants;

[UsedImplicitly]
internal class WebsiteResolver : ITenantResolver<Website>
{
    private readonly IWebsiteService _websiteService;
    private readonly IHttpContextAccessor _context;

    public WebsiteResolver(
        IWebsiteService websiteService,
        IHttpContextAccessor context)
    {
        _websiteService = websiteService;
        _context = context;
    }

    public ValueTask<Website?> Resolve()
    {
        return ValueTask.FromResult(_context.HttpContext is null
            ? default
            : _websiteService.Get(_context.HttpContext.Request.Host.Host));
    }
}
