using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Leightweight.Multitenancy.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Leightweight.Multitenancy;

/// <summary>
/// Utility methods for working with tenants.
/// </summary>
[PublicAPI]
public static class MultitenancyUtilities
{
    /// <summary>
    /// Resolves a <typeparamref name="TTenant"/> for the current dependency injection scope and populates it in the
    /// scope's <see cref="ITenant{TTenant}"/>.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <typeparam name="TTenant">The type of tenant.</typeparam>
    /// <returns>The populated <see cref="ITenant{TTenant}"/>.</returns>
    public static async ValueTask<ITenant<TTenant>> PopulateTenant<TTenant>(IServiceProvider provider)
    {
        ThrowHelper.ThrowIfNull(provider);

        var wrapper = provider.GetRequiredService<TenantWrapper<TTenant>>();
        var resolver = provider.GetRequiredService<ITenantResolver<TTenant>>();

        wrapper.Tenant = await resolver.Resolve();

        return wrapper;
    }
}
