using JetBrains.Annotations;
using Leightweight.Multitenancy;
using Leightweight.Multitenancy.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up multitenancy in an <see cref="IServiceCollection" />.
/// </summary>
[PublicAPI]
public static class MultitenancyServiceCollectionExtensions
{
    /// <summary>
    /// Adds an <see cref="ITenant{TTenant}"/> and related services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="lifetime">The dependency injection service lifetime for the tenant.</param>
    /// <typeparam name="TTenant">The type of tenant.</typeparam>
    /// <returns>An <see cref="IMultitenancyBuilder{TTenant}"/> that can be used to configure the tenant.</returns>
    public static IMultitenancyBuilder<TTenant> AddTenant<TTenant>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TTenant : class
    {
        ThrowHelper.ThrowIfNull(services);

        services.Add(
            ServiceDescriptor.Describe(
                typeof(TenantWrapper<TTenant>),
                typeof(TenantWrapper<TTenant>),
                lifetime));
        services.Add(
            ServiceDescriptor.Describe(
                typeof(ITenant<TTenant>),
                p => p.GetRequiredService<TenantWrapper<TTenant>>(),
                lifetime));

        return new MultitenancyBuilder<TTenant>(services);
    }
}
