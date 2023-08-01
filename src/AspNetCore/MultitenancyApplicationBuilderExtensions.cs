using System;
using JetBrains.Annotations;
using Leightweight.Multitenancy;
using Leightweight.Multitenancy.AspNetCore.Internal;
using Leightweight.Multitenancy.Internal;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods for setting up multitenancy in an <see cref="IApplicationBuilder" />.
/// </summary>
[PublicAPI]
public static class MultitenancyApplicationBuilderExtensions
{
    private const string MissingServiceFormat = "Unable to find the required services. Please add all the required services by calling 'IServiceCollection.{0}<{2}>().{1}<{2}>()' where services are registered in the application startup code.";

    /// <summary>
    /// Adds middleware to the pipeline that resolves the specified <typeparamref name="TTenant"/> so it can be injected
    /// into other services.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <typeparam name="TTenant">The type of tenant to resolve.</typeparam>
    /// <returns>The application builder.</returns>
    public static IApplicationBuilder UseTenant<TTenant>(this IApplicationBuilder app)
        where TTenant : class
    {
        ThrowHelper.ThrowIfNull(app);

        UseTenantCore<TTenant>(app);
        return app;
    }

    private static void UseTenantCore<TTenant>(IApplicationBuilder app)
        where TTenant : class
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            if (scope.ServiceProvider.GetService<ITenant<TTenant>>() is null ||
                scope.ServiceProvider.GetService<ITenantResolver<TTenant>>() is null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        MissingServiceFormat,
                        nameof(MultitenancyServiceCollectionExtensions.AddTenant),
                        nameof(MultitenancyBuilder<TTenant>.WithResolver),
                        typeof(TTenant)));
            }
        }

        app.UseMiddleware<MultitenancyMiddleware<TTenant>>();
    }
}
