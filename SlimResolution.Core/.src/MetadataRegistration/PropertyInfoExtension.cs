using System;
using System.Reflection;
using System.Collections.Generic;

using SlimResolution.Core.ResolutionSourceProcessing;
using SlimResolution.Core.MetadataRegistration.Internals;


namespace SlimResolution.Core.MetadataRegistration;

public static class PropertyInfoExtension
{
    public static void RunRegistration(this IEnumerable<PropertyInfo> propertyInfos,
                                       in MetadataInfo metadataInfo,
                                       ValidateResolutionSource validateResolutionSource,
                                       ResolveMetadataDependency resolveDependency,
                                       RegisterMetadata registerMetada)
    {
        List<Type> ctorArgTypes = [typeof(ValidateResolutionSource), typeof(IDelegateCreator)];
        List<object> ctorArgs = [validateResolutionSource];


        var delegateBuilder = ResolutionDelegateBuilder.Instance;

        List<Type> resolutionDelegateTypes = [];
        List<object> resolutionDelegates = [];

        foreach (var info in propertyInfos)
        {
            if (!info.IsForResolutionDelegate()) continue;

            resolutionDelegateTypes.Add(info.PropertyType);

            var resolutionDelegate = delegateBuilder.BuildDelegate(info.PropertyType, resolveDependency);
            resolutionDelegates.Add(resolutionDelegate);
        }


        ctorArgTypes.AddRange(resolutionDelegateTypes);
        var ctorInfo = metadataInfo.ConcreteType.GetConstructor([..ctorArgTypes])
            ?? throw new MissingMethodException("ctor not found");


        registerMetada(metadataInfo.InterfaceType, s =>
        {
            ctorArgs.Add(resolveDependency(typeof(IDelegateCreator), s));
            ctorArgs.AddRange(resolutionDelegates);

            return ctorInfo.Invoke([.. ctorArgs]);
        });
    }
}