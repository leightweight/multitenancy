using System;
using System.Collections.Generic;
using Leightweight.Multitenancy.Samples.ConsoleApp.Tenants;

namespace Leightweight.Multitenancy.Samples.ConsoleApp.Services;

internal class UserService
{
    private readonly Dictionary<string, User> _users = new(StringComparer.OrdinalIgnoreCase);

    public User GetUser(string name)
    {
        return _users.TryGetValue(name, out var user)
            ? user
            : _users[name] = new(Guid.NewGuid(), name);
    }
}
