using System;

namespace Leightweight.Multitenancy.Samples.AspNetCoreApp.Tenants;

public record Website(Guid Id, string Hostname, string Name, string Body);
