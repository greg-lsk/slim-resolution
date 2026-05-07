using System;
using System.Reflection;
using System.Collections.Generic;

using SlimResolution.Core.ResolutionSourceProcessing;
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
        List<Type> ctorArgTypes = [typeof(ResolutionSourceValidation), typeof(IDelegateCreator)];
        List<object> ctorArgs = [sourceValidation];


        List<Type> resolutionDelegateTypes = [];
        List<object> resolutionDelegates = [];

        var delegateBuilder = ResolutionDelegateBuilder.Instance;

        foreach (var info in propertyInfos)
        {
            if (!info.IsForResolutionDelegate()) continue;

            resolutionDelegateTypes.Add(info.PropertyType);

            var resolutionDelegate = delegateBuilder.BuildDelegate(info.PropertyType, resolution);
            resolutionDelegates.Add(resolutionDelegate);
        }

        ctorArgTypes.AddRange(resolutionDelegateTypes);
        var ctorInfo = metadataInfo.ConcreteType.GetConstructor([..ctorArgTypes])
            ?? throw new MissingMethodException("ctor not found");

        registration(metadataInfo.InterfaceType, s =>
        {
            ctorArgs.Add(resolution(typeof(IDelegateCreator), s));
            ctorArgs.AddRange(resolutionDelegates);

            return ctorInfo.Invoke([.. ctorArgs]);
        });
    }
}