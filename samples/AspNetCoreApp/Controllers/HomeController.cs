using JetBrains.Annotations;
using Leightweight.Multitenancy.Samples.AspNetCoreApp.Tenants;
using Microsoft.AspNetCore.Mvc;

namespace Leightweight.Multitenancy.Samples.AspNetCoreApp.Controllers;

[ApiController]
[Route("/")]
[PublicAPI]
public class HomeController : Controller
{
    private readonly ITenant<Website> _tenant;

    public HomeController(ITenant<Website> tenant)
    {
        _tenant = tenant;
    }

    [HttpGet]
    public ActionResult<HomeIndexResponse> Index()
    {
        if (_tenant.Tenant is not null)
        {
            return new HomeIndexResponse(
                $"Welcome to {_tenant.Tenant.Name}!",
                _tenant.Tenant.Body);
        }

        return new HomeIndexResponse(
            "Welcome to Sample Web Hosting!",
            "Please create your website by using the `POST /websites` endpoint.");
    }
}

[PublicAPI]
public record HomeIndexResponse(string Title, string Body);
