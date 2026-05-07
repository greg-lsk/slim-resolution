using SlimResolution.Core.MetadataRegistration;
using SlimResolution.Core.ResolutionSourceProcessing;
using SlimResolution.Core.ResolutionSourceProcessing.Internals;


namespace SlimResolution.Core;

public abstract class ResolutionMetadataBase
{
    private readonly ResolutionSourceValidation _sourceValidation;

    protected AccessRootServiceProvider AccessSource { get; }


    protected ResolutionMetadataBase(ResolutionSourceValidation sourceValidation,
                                     IDelegateCreator delegateCreator)
    {
        AccessSource = delegateCreator.Create<AccessRootServiceProvider>();
        _sourceValidation = sourceValidation;
    }


    public bool IsLinkedTo(ResolutionSource source) => _sourceValidation(AccessSource(source));
}