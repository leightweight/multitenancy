using System;
using Leightweight.Multitenancy;

var user = new User(Guid.NewGuid(), "jleight");
var wrapper = new TenantWrapper<User> { Tenant = user };
var tenant = (ITenant<User>)wrapper;

Console.WriteLine($"ID: {tenant.Tenant?.Id}");
Console.WriteLine($"Username: {tenant.Tenant?.Username}");

file record User(Guid Id, string Username);
