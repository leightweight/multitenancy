using System;
using System.Collections.Generic;
using System.Linq;
using Leightweight.Multitenancy.Samples.AspNetCoreApp.Tenants;

namespace Leightweight.Multitenancy.Samples.AspNetCoreApp.Services;

public interface IWebsiteService
{
    List<Website> List();
    Website? Get(Guid id);
    Website? Get(string hostname);
    (Website?, int) Create(string hostname, string name, string body);
    Website? Update(Guid id, string name, string body);
    void Delete(Guid id);
}

internal class WebsiteService : IWebsiteService
{
    private readonly Dictionary<Guid, Website> _websites = new();

    public List<Website> List()
        => _websites.Values.ToList();

    public Website? Get(Guid id)
        => _websites.TryGetValue(id, out var w) ? w : default;

    public Website? Get(string hostname)
        => _websites.Values.FirstOrDefault(w => w.Hostname.Equals(hostname, StringComparison.OrdinalIgnoreCase));

    public (Website?, int) Create(string hostname, string name, string body)
    {
        if (Get(hostname) is not null)
            return (default, 409);

        var website = new Website(
            Guid.NewGuid(),
            hostname,
            name,
            body);

        _websites[website.Id] = website;

        return (website, 200);
    }

    public Website? Update(Guid id, string name, string body)
    {
        if (!_websites.TryGetValue(id, out var existing))
            return default;

        _websites[id] = existing with
        {
            Name = name,
            Body = body
        };

        return _websites[id];
    }

    public void Delete(Guid id)
    {
        _websites.Remove(id);
    }
}
