using System;
using System.Reflection;
using System.Collections.Generic;

using SlimResolution.Core.MetadataRegistration.Internals.Utils;


namespace SlimResolution.Core.MetadataRegistration.Internals;

internal static class RegistrationSteps
{
    internal static IEnumerable<(Type InterfaceType, Type ConcreteType)> FilterByMetadata(this IEnumerable<Type> types)
    {
        List<(Type, Type)> typesList = [];

        foreach (var type in types)
        {
            if (type.IsNotConcreteClass()) continue;

            var closedGenericInterfaceType = type.TryGetClosedGenericInterface(typeof(IResolutionMetadata<>));
            if (closedGenericInterfaceType is null) continue;

            typesList.Add((closedGenericInterfaceType, type));
        }

        return typesList;
    }

    internal static void OnEach(this IEnumerable<(Type InterfaceType, Type ConcreteType)> metadataInfos,
                                Register registerDelegate)
    {
        foreach (var metadataInfo in metadataInfos) registerDelegate(metadataInfo);
    }

    internal static void RunRegistration(this IEnumerable<PropertyInfo> propertyInfos,
                                         in (Type InterfaceType, Type ConcreteType) metadataInfo,
                                         Registration registration,
                                         IServiceResolver resolver)
    {
        List<Type> resolutionTypes = [];
        List<Delegate> resolutionDelegates = [];
        
        var delegateBuilder = ResolutionDelegateBuilder.Instance;

        foreach (var info in propertyInfos)
        {
            if (!info.IsForResolutionDelegate()) continue;

            resolutionTypes.Add(info.PropertyType);

            var resolutionDelegate = delegateBuilder.BuildDelegate(info.PropertyType, resolver);
            resolutionDelegates.Add(resolutionDelegate);
        }

        var ctorInfo = metadataInfo.ConcreteType.GetConstructor([.. resolutionTypes])
            ?? throw new MissingMethodException("ctor not found");

        registration(metadataInfo.InterfaceType, () => ctorInfo.Invoke([.. resolutionDelegates]));
    }
}