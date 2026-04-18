using System;
using SlimResolution.Core.ResolutionSourceProcessing;


namespace SlimResolution.Core;

public abstract class ResolutionMetadataBase
{
    private readonly Func<object, bool> _validateSource;

    protected Func<ResolutionSource, object> AccessSource { get; }


    protected ResolutionMetadataBase(Func<object, bool> validateSource)
    {
        AccessSource = new SourceAccessor().Accessor;
        _validateSource = validateSource;
    }


    public bool IsLinkedTo(ResolutionSource source) => _validateSource(AccessSource(source));
}