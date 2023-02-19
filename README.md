# Leightweight.Multitenancy

[![CI](https://github.com/leightweight/multitenancy/workflows/CI/badge.svg)](https://github.com/leightweight/multitenancy/actions?query=workflow%3ACI)
[![NuGet](https://img.shields.io/nuget/vpre/Leightweight.Multitenancy.svg?label=NuGet)](https://www.nuget.org/packages/Leightweight.Multitenancy/)

Simple multitenancy for your .NET project.

## Overview

In a multitenant application, knowing the tenant for the current scope is extremely important.
Each tenant may have its own set of resources, such as database, storage accounts, and cache instances.
Tenants may even have different functionality depending on how they're configured, or the permissions they've been granted.

This is where Leightweight.Multitenancy comes in.
It provides a simple, extensible way to identify and inject the current tenant without all the boilerplate.

## Installation

To install the NuGet package, you can use the following `dotnet` command:

```shell
dotnet add package Leightweight.Multitenancy.AspNetCore
```

Or, via the NuGet package manager console:

```shell
Install-Package Leightweight.Multitenancy.AspNetCore
```

If you're using this in an app that doesn't use ASP.NET Core, you can install the base `Leightweight.Multitenancy` package, instead.

## Usage

To set up a tenant for dependency injection, you can register it with the .NET dependency injection container using the `AddTenant<TTenant>().WithResolver<TResolver>()` extension methods.
Here's an example of how to do this:

```csharp
services
    .AddTenant<MyTenant>()
    .WithResolver<MyTenantResolver>();
```

This will register the `MyTenant` tenant and its `MyTenantResolver` resolver with the dependency injection container.
The `MyTenant` class doesn't require anything special, but the `MyTenantResolver` will need to implement the `ITenantResolver<MyTenant>` interface.

If you're writing an ASP.NET Core application, there is an included middleware implementation that will resolve the tenant.
You can register the middleware like this:

```csharp
app.UseTenant<MyTenant>();
```

This will add the middleware to the request pipeline and the tenant will be available via dependency injection in any middleware, controllers, or other services that are injected after that point.
For more details, check out the [`samples/AspNetCoreApp/README.md`](/leightweight/multitenancy/blob/main/samples/AspNetCoreApp/README.md) file.

To use `MyTenant` in your application, you can inject an `ITenant<MyTenant>` interface into your classes. The `ITenant<T>` interface is a generic interface that provides access to the current scope's tenant instance.
Here's an example of how to do this:

```csharp
public class MyService
{
    private readonly ITenant<MyTenant> _tenant;

    public MyService(ITenant<MyTenant> tenant)
    {
        _tenant = tenant;
    }

    public void DoSomething()
    {
        if (_tenant.Tenant is null)
        {
            // handle the case where the tenant could not be resolved
        }
        else
        {
            await using var connection = new SqlConnection(_tenant.Tenant.SqlConnectionString);
            // use the connection to execute queries or perform other operations
        }
    }
}
```

In this example, the `MyService` class has a dependency on the `ITenant<MyTenant>` interface.
When the service is constructed by the dependency injection container, the current scope's instance of `ITenant<MyTenant>` is injected into the service.

The `DoSomething` method uses the `Tenant` property of the `ITenant<MyTenant>` to retrieve the current tenant.
If the tenant could not be resolved, the `Tenant` property will be null.

### Implementing a Tenant Resolver

To implement a custom tenant resolver, you'll need to create a class that implements the `ITenantResolver<TTenant>` interface.
This interface is generic, so you'll need to specify the type of tenant that your resolver will be able to resolve.

Here's an example implementation of the `ITenantResolver<TTenant>` interface:

```csharp
public class MyTenantResolver : ITenantResolver<MyTenant>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MyTenantResolver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async ValueTask<MyTenant?> Resolve()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            // handle the case where the HTTP context is not available
        }

        // TODO: resolve the tenant from the HTTP context

        return new MyTenant();
    }
}
```

In this example, the `MyTenantResolver` class implements the `ITenantResolver<MyTenant>` interface.
It has a dependency on an `IHttpContextAccessor` instance, which is used to retrieve the current `HttpContext`.

The `Resolve` method is responsible for resolving the tenant.
In this example, it simply returns a new instance of the `MyTenant` class, but in a real implementation, you would need to use the `HttpContext` or some other source to identify and resolve the current tenant.

If you need to resolve the tenant from the current request's `HttpContext`, you can inject an `IHttpContextAccessor`.
You'll need to remember to add the `IHttpContextAccessor` to your service collection setup.
Here's an example of how to do this:

```csharp
services.AddHttpContextAccessor();
```

This will register the `IHttpContextAccessor` with the dependency injection container, and you'll be able to use it to resolve the current `HttpContext` in your tenant resolver.

## Samples

The `samples` directory contains two example applications that serve as a good starting point for learning how to use this library.
One is a console application that resolves two different tenants at the beginning of each application loop and uses the `MultitenancyUtilities.PopulateTenant` method to manually populate the tenant with the tenant resolver.
The other is an ASP.NET core application that resolves the current tenant based on the request's hostname.
