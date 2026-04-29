using System;
using System.Reflection;
using System.Collections.Generic;

using SlimResolution.Core.MetadataRegistration.Internals.Utils;


namespace SlimResolution.Core.MetadataRegistration;

public static class RunRegistrationExtension
{
    public static void RunRegistration(this IEnumerable<PropertyInfo> propertyInfos,
                                       in MetadataInfo metadataInfo,
                                       ResolutionSourceValidation sourceValidation,
                                       Resolution resolution,
                                       Registration registration)
    {
        List<Type> ctorArgTypes = [typeof(ResolutionSourceValidation)];
        List<object> ctorArgs = [sourceValidation];

        var delegateBuilder = ResolutionDelegateBuilder.Instance;

        foreach (var info in propertyInfos)
        {
            if (!info.IsForResolutionDelegate()) continue;

            ctorArgTypes.Add(info.PropertyType);

            var resolutionDelegate = delegateBuilder.BuildDelegate(info.PropertyType, resolution);
            ctorArgs.Add(resolutionDelegate);
        }

        var ctorInfo = metadataInfo.ConcreteType.GetConstructor([.. ctorArgTypes])
            ?? throw new MissingMethodException("ctor not found");

        registration(metadataInfo.InterfaceType, () => ctorInfo.Invoke([.. ctorArgs]));
    }
}