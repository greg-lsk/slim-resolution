using System;


namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal sealed class ComposerRegistrator<TProvider>(ResolveServiceFromType<TProvider> resolveFromType,
                                                     RegisterService<TProvider> register) 
    : RegistratorBase<TProvider>(resolveFromType, register)
    where TProvider : notnull
{
    public override void OnNext(RegistrationInfo info)
    {
        var ctor = info.ConcreteComposerType.GetConstructor(
        [
            typeof(object),
            typeof(ValidateResolutionSource),
            info.AbstractMetadataType
        ]) ?? throw new Exception("Ctor not found");

        Register(info.AbstractComposerType, p => ctor.Invoke(
        [
            p,
            (ValidateResolutionSource) (o => o is TProvider),
            ResolveFromType(p, info.AbstractMetadataType)
        ]));
    }
}