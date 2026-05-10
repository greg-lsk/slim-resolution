using System;
using System.Collections.Generic;


namespace SlimResolution.Core.MetadataTypeDiscovery.Internals;

internal static class TypeExtensions
{
    private static Type? GetClosedGenericInterface(this Type type, Type openGenericType)
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

    private static bool IsNotConcreteClass(this Type type)
    {
        return type.IsValueType || type.IsAbstract || type.IsInterface;
    }

    internal static IEnumerable<RegistrationInfo> FilterByMetadata(this IEnumerable<Type> types)
    {
        List<RegistrationInfo> typesList = [];

        foreach (var type in types)
        {
            if (type.IsNotConcreteClass()) continue;

            var closedGenericInterfaceType = type.GetClosedGenericInterface(typeof(IResolutionMetadata<>));
            if (closedGenericInterfaceType is null) continue;

            typesList.Add(RegistrationInfo.Create(closedGenericInterfaceType, type));
        }

        return typesList;
    }
}