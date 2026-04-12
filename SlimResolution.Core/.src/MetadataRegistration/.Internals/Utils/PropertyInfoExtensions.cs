using System.Reflection;


namespace SlimResolution.Core.MetadataRegistration.Internals.Utils;

internal static class PropertyInfoExtensions
{
    internal static bool IsForResolutionDelegate(this PropertyInfo info)
    {   
        return info.PropertyType.IsGenericType
               && info.PropertyType.GetGenericTypeDefinition() == typeof(Resolution<>);
    }
}