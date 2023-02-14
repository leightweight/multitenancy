using System.Threading.Tasks;
using Leightweight.Multitenancy.Internal;
using Microsoft.AspNetCore.Http;

namespace Leightweight.Multitenancy.AspNetCore.Internal;

/// <summary>
/// Default middleware to resolve <typeparamref name="TTenant"/>s and add them to the scoped
/// <see cref="TenantWrapper{TTenant}"/>.
/// </summary>
/// <typeparam name="TTenant">The type of tenant.</typeparam>
internal sealed class MultitenancyMiddleware<TTenant>
    where TTenant : class
{
    private readonly RequestDelegate _next;

    public MultitenancyMiddleware(RequestDelegate next)
    {
        ThrowHelper.ThrowIfNull(next);

        _next = next;
    }

    /// <summary>
    /// Resolves the <typeparamref name="TTenant"/> into the scope's <see cref="TenantWrapper{TTenant}"/> and then
    /// continues the middleware pipeline.
    /// </summary>
    /// <param name="context">The context for the current request.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        ThrowHelper.ThrowIfNull(context);

        await MultitenancyUtilities.PopulateTenant<TTenant>(context.RequestServices);
        await _next.Invoke(context);
    }
}
