using Microsoft.Extensions.DependencyInjection;

namespace Leightweight.Multitenancy.Internal;

/// <summary>
/// Default implementation of <see cref="IMultitenancyBuilder{TTenant}"/>.
/// </summary>
internal sealed class MultitenancyBuilder<TTenant> : IMultitenancyBuilder<TTenant>
    where TTenant : class
{
    /// <inheritdoc />
    public IServiceCollection Services { get; }

    /// <summary>
    /// Constructs a new multitenancy configuration object and sets its
    /// <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services"></param>
    public MultitenancyBuilder(IServiceCollection services)
    {
        ThrowHelper.ThrowIfNull(services);

        Services = services;
    }

    /// <inheritdoc />
    public IMultitenancyBuilder<TTenant> WithResolver<TResolver>(
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TResolver : class, ITenantResolver<TTenant>
    {
        Services.Add(
            ServiceDescriptor.Describe(
                typeof(ITenantResolver<TTenant>),
                typeof(TResolver),
                lifetime));

        return this;
    }
}
