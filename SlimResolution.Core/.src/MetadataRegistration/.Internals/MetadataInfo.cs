using System;
using System.Reflection;


namespace SlimResolution.Core.MetadataRegistration.Internals;

internal record struct MetadataInfo(Type InterfaceType, Type ConcreteType)
{
    private readonly BindingFlags Binding => BindingFlags.Public
                                             | BindingFlags.NonPublic
                                             | BindingFlags.Instance;


    internal static MetadataInfo Create(Type InterfaceType, Type ConcreteType)
    {
        return new(InterfaceType, ConcreteType);
    }


    internal readonly PropertyInfo[] GetResolutionProperties() => ConcreteType.GetProperties(Binding);
}