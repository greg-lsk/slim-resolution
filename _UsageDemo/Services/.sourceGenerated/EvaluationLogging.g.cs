using _UsageDemo.Services;
using SlimResolution.Core;


namespace _UsageDemo.Services;

internal readonly partial struct EvaluationLogging 
{
    private readonly IResolutionMetadata<EvaluationLogging> _resolutionMetadata;
    private readonly IResolutionContext _resolutionContext;


    private partial IPseudoLog Logger 
        => (_resolutionMetadata as EvaluationLoggingMetadata).LoggerResolution(_resolutionContext);


    internal EvaluationLogging(IResolutionMetadata<EvaluationLogging> metadata,
                               IResolutionContext context)
    {
        _resolutionMetadata = metadata;
        _resolutionContext = context;
    }
}