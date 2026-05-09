using System;
using System.Reflection;


namespace SlimResolution.Core.DependencyInjectionUtils;

public readonly struct MetadataInfo
{
    private readonly BindingFlags Binding => BindingFlags.Public
                                             | BindingFlags.NonPublic
                                             | BindingFlags.Instance;

    public Type InterfaceType { get; }
    public Type ConcreteType { get; }


    private MetadataInfo(Type interfaceType, Type concreteType)
    {
        InterfaceType = interfaceType;
        ConcreteType = concreteType;
    }
    internal static MetadataInfo Create(Type InterfaceType, Type ConcreteType)
    {
        return new(InterfaceType, ConcreteType);
    }


    public readonly PropertyInfo[] GetResolutionProperties() => ConcreteType.GetProperties(Binding);
}