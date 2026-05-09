using System.Reflection;


namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal static class PropertyInfoExtensions
{
    internal static bool IsForResolutionDelegate(this PropertyInfo info)
    {   
        return info.PropertyType.IsGenericType
               && info.PropertyType.GetGenericTypeDefinition() == typeof(Resolution<>);
    }
}