using SlimResolution.Core;
using SlimResolution.Core.ErrorHandling;


namespace _UsageDemo.Services;

internal class EvaluationLoggingMetadata : ResolutionMetadataBase, IResolutionMetadata<EvaluationLogging>
{
    private Resolution<IPseudoLog> _loggerResolution { get; }


    public EvaluationLoggingMetadata(
        Func<object, bool> validateSource,
        Resolution<IPseudoLog> loggerResolution) : base(validateSource)
    {
        _loggerResolution = loggerResolution;
    }


    public EvaluationLogging Materialize(ResolutionSource source)
    {
        return new(this, source);
    }

    internal IPseudoLog LoggerResolution(ResolutionSource source)
    {
        return _loggerResolution(AccessSource(source));
    }
}