using _UsageDemo.Services;
using SlimResolution.Core;
using SlimResolution.Core.ErrorHandling.StaticThrowHelpers;


namespace _UsageDemo.Services;

internal readonly partial struct EvaluationLogging 
{
    private readonly IResolutionMetadata<EvaluationLogging> _resolutionMetadata;
    private readonly ResolutionSource _resolutionSource;


    private partial IPseudoLog Logger
        => (_resolutionMetadata as EvaluationLoggingMetadata)!.LoggerResolution(_resolutionSource);


    internal EvaluationLogging(IResolutionMetadata<EvaluationLogging> metadata,
                               ResolutionSource resolutionSource)
    {
        InvalidArgumentException.ThrowIfNotBound<EvaluationLogging, EvaluationLoggingMetadata>(metadata);
        InvalidArgumentException.ThrowIfUnlinked(resolutionSource, metadata);

        _resolutionMetadata = metadata;
        _resolutionSource = resolutionSource;
    }
}