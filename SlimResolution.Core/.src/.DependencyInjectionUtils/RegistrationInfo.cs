using System;
using System.Reflection;

using SlimResolution.Core.ResolutionComposition.Internals;


namespace SlimResolution.Core.DependencyInjectionUtils;

public readonly struct RegistrationInfo
{
    private readonly BindingFlags Binding => BindingFlags.Public
                                             | BindingFlags.NonPublic
                                             | BindingFlags.Instance;

    internal Type AbstractMetadataType { get; }
    internal Type ConcreteMetadataType { get; }

    internal Type CompositionTargetType => AbstractMetadataType.GenericTypeArguments[0];

    internal Type AbstractComposerType => typeof(IComposer<>).MakeGenericType(CompositionTargetType);
    internal Type ConcreteComposerType => typeof(Composer<>).MakeGenericType(CompositionTargetType);


    private RegistrationInfo(Type abstractMetadataType, Type concreteMetadataType)
    {
        AbstractMetadataType = abstractMetadataType;
        ConcreteMetadataType = concreteMetadataType;
    }
    internal static RegistrationInfo Create(Type abstractMetadataType, Type concreteMetadataType)
    {
        return new(abstractMetadataType, concreteMetadataType);
    }


    public readonly PropertyInfo[] GetResolutionProperties() => ConcreteMetadataType.GetProperties(Binding);
}