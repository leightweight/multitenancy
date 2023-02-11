using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Leightweight.Multitenancy;

/// <summary>
/// Used to resolve <typeparamref name="TTenant"/> tenants.
/// </summary>
/// <typeparam name="TTenant">The type of tenant being resolved.</typeparam>
[PublicAPI]
public interface ITenantResolver<TTenant>
{
    /// <summary>
    /// Resolves a <typeparamref name="TTenant"/> tenant for the current scope.
    /// </summary>
    /// <returns>
    /// A <typeparamref name="TTenant"/> instance for the scope, or
    /// <see langword="null"/> if one cannot be resolved.
    /// </returns>
#if NETSTANDARD2_0
    Task<TTenant?> Resolve();
#elif NET462
    Task<TTenant?> Resolve();
#else
    ValueTask<TTenant?> Resolve();
#endif
}
