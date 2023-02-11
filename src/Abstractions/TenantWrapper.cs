using JetBrains.Annotations;

namespace Leightweight.Multitenancy.Abstractions;

/// <summary>
/// Wraps a <typeparamref name="TTenant"/> so it can be used as an
/// <see cref="ITenant{TTenant}" />.
/// </summary>
/// <typeparam name="TTenant"></typeparam>
[PublicAPI]
public class TenantWrapper<TTenant> : ITenant<TTenant>
{
    /// <summary>
    ///
    /// </summary>
    public TTenant? Tenant { get; set; }
}
