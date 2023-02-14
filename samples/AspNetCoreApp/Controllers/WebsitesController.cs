using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Leightweight.Multitenancy.Samples.AspNetCoreApp.Services;
using Leightweight.Multitenancy.Samples.AspNetCoreApp.Tenants;
using Microsoft.AspNetCore.Mvc;

namespace Leightweight.Multitenancy.Samples.AspNetCoreApp.Controllers;

[ApiController]
[Route("/[controller]")]
[PublicAPI]
public class WebsitesController : Controller
{
    private readonly IWebsiteService _websiteService;
    private readonly ITenant<Website> _tenant;

    public WebsitesController(
        IWebsiteService websiteService,
        ITenant<Website> tenant)
    {
        _websiteService = websiteService;
        _tenant = tenant;
    }

    [HttpGet]
    public ActionResult<List<Website>> List()
    {
        return _tenant.Tenant is null
            ? _websiteService.List()
            : new List<Website>
            {
                _tenant.Tenant
            };
    }

    [HttpGet("{id:guid}")]
    public ActionResult<Website?> Get(
        [FromRoute] Guid id)
    {
        if (_tenant.Tenant is null)
            return _websiteService.Get(id);
        if (_tenant.Tenant.Id == id)
            return _tenant.Tenant;
        return NotFound();
    }

    [HttpGet("me")]
    public ActionResult<Website?> Get()
    {
        if (_tenant.Tenant is not null)
            return _tenant.Tenant;
        return NotFound();
    }

    [HttpPost]
    public ActionResult<Website?> Create(
        [FromBody] WebsitesCreateRequest request)
    {
        if (_tenant.Tenant is not null)
            return NotFound();

        var (result, response) = _websiteService.Create(
            request.Hostname,
            request.Name,
            request.Body);

        if (result is not null)
            return result;

        return StatusCode(response);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<Website?> Update(
        [FromRoute] Guid id,
        [FromBody] WebsitesUpdateRequest request)
    {
        if (_tenant.Tenant is not null && _tenant.Tenant.Id != id)
            return NotFound();

        return _websiteService.Update(
            id,
            request.Name,
            request.Body);
    }

    [HttpPut("me")]
    public ActionResult<Website?> Update(
        [FromBody] WebsitesUpdateRequest request)
    {
        if (_tenant.Tenant is null)
            return NotFound();

        return _websiteService.Update(
            _tenant.Tenant.Id,
            request.Name,
            request.Body);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(
        [FromRoute] Guid id)
    {
        if (_tenant.Tenant is not null && _tenant.Tenant.Id != id)
            return NotFound();

        _websiteService.Delete(id);
        return Ok();
    }

    [HttpDelete("me")]
    public IActionResult Delete()
    {
        if (_tenant.Tenant is null)
            return NotFound();

        _websiteService.Delete(_tenant.Tenant.Id);
        return Ok();
    }
}

[PublicAPI]
public record WebsitesCreateRequest(string Hostname, string Name, string Body);

[PublicAPI]
public record WebsitesUpdateRequest(string Name, string Body);
