using SlimResolution.Core.MetadataRegistration;
using SlimResolution.Core.ResolutionSourceProcessing;


namespace SlimResolution.Core;

public abstract class ResolutionMetadataBase
{
    private readonly ResolutionSourceValidation _sourceValidation;

    protected AccessRootServiceProvider AccessSource { get; }


    protected ResolutionMetadataBase(ResolutionSourceValidation sourceValidation)
    {
        AccessSource = SourceAccessor.Instance.Accessor;
        _sourceValidation = sourceValidation;
    }


    public bool IsLinkedTo(ResolutionSource source) => _sourceValidation(AccessSource(source));
}