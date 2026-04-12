using System;


namespace SlimResolution.Core.MetadataRegistration.Internals.Utils;

internal static class TypeExtensions
{
    internal static Type? TryGetClosedGenericInterface(this Type type, Type openGenericType)
    {
        foreach (var implementedInterface in type.GetInterfaces())
        {
            if (implementedInterface.IsGenericType
                && implementedInterface.GetGenericTypeDefinition() == openGenericType)
            {
                return implementedInterface;
            }
        }

        return null;
    }

    internal static bool IsNotConcreteClass(this Type type)
    {
        return type.IsValueType || type.IsAbstract || type.IsInterface;
    }
}