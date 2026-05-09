using System;

using SlimResolution.Core.ResolutionComposition.Internals;


namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal sealed class ComposerRegistrator<TProvider>(ResolveMetadataDependency resolveMetadataDependency,
                                                   RegisterMetadata registerMetadata) 
    : RegistratorBase(resolveMetadataDependency, registerMetadata)
{
    public override void OnNext(MetadataInfo info)
    {
        var compositionTargetType = info.InterfaceType.GetGenericArguments()[0];

        var abstractComposerType = typeof(IComposer<>).MakeGenericType(compositionTargetType);
        var concreteComposerType = typeof(Composer<>).MakeGenericType(compositionTargetType);

        var abstractMetadataType = info.InterfaceType;

        var ctor = concreteComposerType.GetConstructor(
        [
            typeof(object),
            typeof(ValidateResolutionSource),
            abstractMetadataType
        ]) ?? throw new Exception("Ctor not found");

        RegisterMetadata(abstractComposerType, p => ctor.Invoke(
        [
            p,
            (ValidateResolutionSource) (o => o is TProvider),
            ResolveMetadataDependency(abstractMetadataType, p)
        ]));
    }
}