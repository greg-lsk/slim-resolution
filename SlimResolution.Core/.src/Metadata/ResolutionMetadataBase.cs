using SlimResolution.Core.MetadataRegistration;
using SlimResolution.Core.ResolutionSourceProcessing;
using SlimResolution.Core.ServiceProviderAbstractions;


namespace SlimResolution.Core.Metadata;

public abstract class ResolutionMetadataBase
{
    private readonly ValidateResolutionSource _sourceValidation;

    protected AccessRootServiceProvider AccessSource { get; }


    protected ResolutionMetadataBase(ValidateResolutionSource sourceValidation,
                                     IDelegateCreator delegateCreator)
    {
        AccessSource = delegateCreator.Create<AccessRootServiceProvider>();
        _sourceValidation = sourceValidation;
    }


    public bool IsLinkedTo(ResolutionSource source) => _sourceValidation(AccessSource(source));
}