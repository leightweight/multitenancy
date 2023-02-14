# ASP.NET Core Sample

This sample shows how to set up multitenancy in an ASP.NET Core application.

## About

This sample is modeled after a web hosting service that allows customers to bring their own vanity domain.
It defines a `Website` tenant which has a unique ID, a hostname, a name, and body content.

When accessing the site through the default hostname (localhost), it allows you to perform CRUD operations against an in-memory collection of `Website`s on the `/websites` endpoint.
Once you've created some websites, you can access the site through one of those website's hostnames and the website's content are returned on the `/` route.

## Try it Out

First, start running the service by running `dotnet run` in your terminal.
Once the service is up and running, you can confirm that there aren't any websites defined:

```shell
curl http://localhost:5000/websites
```

This should result in an empty array.
Then create a new website:

```shell
curl -i -X POST \
  -H 'Content-Type: application/json' \
  -d '{"hostname": "test", "name": "Test Website", "body": "Welcome to the test website."}' \
  http://localhost:5000/websites
```

Once the test website is created, request the homepage with the test hostname:

```shell
curl -H 'Host: test' http://localhost:5000
```

You should receive a response containing the test website data instead of the default service homepage which normally lets you know to use the `/websites` endpoint.

## How it Works

It starts with defining a tenant--`Website` in this example.
To keep it simple, it's just a `record` containing a few example properties.
The important part is that it contains a `Hostname` property which is what will be used to find the correct tenant for each HTTP request.

Then there's the `WebsiteService`.
Normally this service would make requests to wherever your tenants are stored--a redis cache, an external REST API, or a database, for example.
In this case, it just stores all of the `Website`s in an in-memory collection.

After that, there's a `WebsiteResolver`.
The `WebsiteResolver` has the `WebsiteService` and an `IHttpContextAccessor` injected into it so that it can look up the `Website` based on the hostname in the HTTP request.
The resolver might also handle something like converting the service's output type into the actual tenant type if they are not the same.
For simplicity, the `WebsiteService` already operates on the tenant type directly.

To get it all set up, the `Program.cs` registers everything into the service collection.
The key part being:

```csharp
builder.Services
    .AddTenant<Website>()
    .WithResolver<WebsiteResolver>();
```

This sets up all the multitenancy services so that `ITenant<Website>` can be injected into the controllers.
Then the middleware that actually resolves the tenant gets added to the pipeline:

```csharp
app.UseTenant<Website>();
```

Keep in mind that the middleware pipeline runs in the order it is defined.
If you base your tenant off something like the `HttpContext.User`, you'll need to call `UseTenant` after `UseAuthentication` or else the user details won't be available for the tenant resolver.
Similarly, if you're using your tenant to determine whether a request is authorized, make sure you call `UseAuthorization` _after_ calling `UseTenant` or your tenant won't be available for the authorization middleware.

Lastly, the controllers have take in an `ITenant<Website>` as a constructor parameter so that it will be injected automatically via dependency injection.
The controller methods can then use the tenant without having to manually look it up via the request hostname.
If the `ITenant<Website>.Tenant` is `null`, the request's hostname doesn't match any defined `Website`s.
