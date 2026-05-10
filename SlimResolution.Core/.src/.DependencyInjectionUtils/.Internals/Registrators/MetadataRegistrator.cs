using System;
using System.Collections.Generic;

using SlimResolution.Core.ResolutionSourceProcessing;
using SlimResolution.Core.MetadataTypeDiscovery.Internals;


namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal sealed class MetadataRegistrator<TProvider>(ResolveServiceFromType<TProvider> resolveMetadataDependency,
                                                     RegisterService<TProvider> registerMetadata) 
    : RegistratorBase<TProvider>(resolveMetadataDependency, registerMetadata) 
    where TProvider : notnull
{
    public override void OnNext(RegistrationInfo info)
    {
        var propertyInfos = info.GetResolutionProperties();

        List <Type> ctorArgTypes = [typeof(IDelegateCreator)];
        List<object> ctorArgs = [];


        var delegateBuilder = ResolutionDelegateBuilder<TProvider>.Instance;

        List<Type> resolutionDelegateTypes = [];
        List<object> resolutionDelegates = [];

        foreach (var propertyInfo in propertyInfos)
        {
            if (!propertyInfo.IsForResolutionDelegate()) continue;

            resolutionDelegateTypes.Add(propertyInfo.PropertyType);

            var resolutionDelegate = delegateBuilder.BuildDelegate(propertyInfo.PropertyType, ResolveFromType);
            resolutionDelegates.Add(resolutionDelegate);
        }


        ctorArgTypes.AddRange(resolutionDelegateTypes);
        var ctorInfo = info.ConcreteMetadataType.GetConstructor([.. ctorArgTypes])
            ?? throw new MissingMethodException("ctor not found");


        Register(info.AbstractMetadataType, s =>
        {
            ctorArgs.Add(ResolveFromType(s, typeof(IDelegateCreator)));
            ctorArgs.AddRange(resolutionDelegates);

            return ctorInfo.Invoke([.. ctorArgs]);
        });
    }
}