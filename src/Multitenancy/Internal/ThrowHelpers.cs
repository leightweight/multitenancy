using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Leightweight.Multitenancy.Internal
{
    internal static class ThrowHelper
    {
        internal static void ThrowIfNull(
            // ReSharper disable once UseNullableReferenceTypesAnnotationSyntax
            [NotNull] object? argument,
            [CallerArgumentExpression("argument")] string? paramName = null)
        {
            if (argument is null)
                throw new ArgumentNullException(paramName);
        }
    }
}

#if !NETCOREAPP2_1_OR_GREATER
namespace System.Runtime.CompilerServices
{
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        public string ParameterName { get; }

        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }
    }
}
#endif
