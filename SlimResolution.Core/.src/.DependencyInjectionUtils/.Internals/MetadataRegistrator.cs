using System;
using System.Collections.Generic;

using SlimResolution.Core.ResolutionSourceProcessing;


namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal sealed class MetadataRegistrator(ResolveMetadataDependency resolveMetadataDependency,
                                        RegisterMetadata registerMetadata) 
    : RegistratorBase(resolveMetadataDependency, registerMetadata)
{
    public override void OnNext(MetadataInfo info)
    {
        var propertyInfos = info.GetResolutionProperties();

        List <Type> ctorArgTypes = [typeof(IDelegateCreator)];
        List<object> ctorArgs = [];


        var delegateBuilder = ResolutionDelegateBuilder.Instance;

        List<Type> resolutionDelegateTypes = [];
        List<object> resolutionDelegates = [];

        foreach (var propertyInfo in propertyInfos)
        {
            if (!propertyInfo.IsForResolutionDelegate()) continue;

            resolutionDelegateTypes.Add(propertyInfo.PropertyType);

            var resolutionDelegate = delegateBuilder.BuildDelegate(propertyInfo.PropertyType, ResolveMetadataDependency);
            resolutionDelegates.Add(resolutionDelegate);
        }


        ctorArgTypes.AddRange(resolutionDelegateTypes);
        var ctorInfo = info.ConcreteType.GetConstructor([.. ctorArgTypes])
            ?? throw new MissingMethodException("ctor not found");


        RegisterMetadata(info.InterfaceType, s =>
        {
            ctorArgs.Add(ResolveMetadataDependency(typeof(IDelegateCreator), s));
            ctorArgs.AddRange(resolutionDelegates);

            return ctorInfo.Invoke([.. ctorArgs]);
        });
    }
}