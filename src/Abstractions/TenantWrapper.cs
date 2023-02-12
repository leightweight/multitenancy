using JetBrains.Annotations;

namespace Leightweight.Multitenancy;

/// <summary>
/// Wraps a <typeparamref name="TTenant"/> so it can be used as an <see cref="ITenant{TTenant}" />.
/// </summary>
/// <typeparam name="TTenant"></typeparam>
[PublicAPI]
public class TenantWrapper<TTenant> : ITenant<TTenant>
{
    /// <summary>
    /// The <typeparamref name="TTenant"/> for the configured scope, or <see langword="null"/> if one wasn't resolved.
    /// </summary>
    public TTenant? Tenant { get; set; }
}
