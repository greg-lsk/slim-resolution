using System;
using System.Reflection;
using System.Collections.Generic;

using SlimResolution.Core.MetadataRegistration.Internals.Utils;


namespace SlimResolution.Core.MetadataRegistration;

public static class RunRegistrationExtension
{
    public static void RunRegistration(this IEnumerable<PropertyInfo> propertyInfos,
                                       in MetadataInfo metadataInfo,
                                       Registration registration,
                                       Resolution resolution)
    {
        List<Type> resolutionTypes = [];
        List<Delegate> resolutionDelegates = [];

        var delegateBuilder = ResolutionDelegateBuilder.Instance;

        foreach (var info in propertyInfos)
        {
            if (!info.IsForResolutionDelegate()) continue;

            resolutionTypes.Add(info.PropertyType);

            var resolutionDelegate = delegateBuilder.BuildDelegate(info.PropertyType, resolution);
            resolutionDelegates.Add(resolutionDelegate);
        }

        var ctorInfo = metadataInfo.ConcreteType.GetConstructor([.. resolutionTypes])
            ?? throw new MissingMethodException("ctor not found");

        registration(metadataInfo.InterfaceType, () => ctorInfo.Invoke([.. resolutionDelegates]));
    }
}