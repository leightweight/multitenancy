using JetBrains.Annotations;

namespace Leightweight.Multitenancy;

/// <summary>
/// Used to retrieve configured <typeparamref name="TTenant"/> tenant.
/// </summary>
/// <typeparam name="TTenant">The type of tenant being requested.</typeparam>
[PublicAPI]
public interface ITenant<out TTenant>
{
    /// <summary>
    /// The <typeparamref name="TTenant"/> tenant for the current scope.
    /// </summary>
    TTenant? Tenant { get; }
}
