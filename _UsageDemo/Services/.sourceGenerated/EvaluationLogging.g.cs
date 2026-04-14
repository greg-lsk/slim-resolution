using _UsageDemo.Services;
using SlimResolution.Core;


namespace _UsageDemo.Services;

internal readonly partial struct EvaluationLogging 
{
    private IResolutionMetadata<EvaluationLogging> ResolutionMetadata { get; init; }
    private IResolutionContext ResolutionContext { get; init; }


    private partial IPseudoLog Logger 
        => (ResolutionMetadata as EvaluationLoggingMetadata).LoggerResolution(ResolutionContext);


    internal EvaluationLogging(IResolutionMetadata<EvaluationLogging> metadata,
                               IResolutionContext context)
    {
        ResolutionMetadata = metadata;
        ResolutionContext = context;
    }
}