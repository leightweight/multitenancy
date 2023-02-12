using JetBrains.Annotations;
using Leightweight.Multitenancy;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides access to methods for configuring a tenant.
/// </summary>
[PublicAPI]
public interface IMultitenancyBuilder<TTenant>
{
    /// <summary>
    /// Provides access to the <see cref="IServiceCollection"/> that was passed to this object's constructor.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Adds an <see cref="ITenantResolver{TTenant}"/> and related services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="lifetime">The dependency injection service lifetime for the tenant resolver.</param>
    /// <typeparam name="TResolver">The type of tenant resolver.</typeparam>
    /// <returns>An <see cref="IMultitenancyBuilder{TTenant}"/> that can be used to configure the tenant.</returns>
    IMultitenancyBuilder<TTenant> WithResolver<TResolver>(
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TResolver : class, ITenantResolver<TTenant>;
}
