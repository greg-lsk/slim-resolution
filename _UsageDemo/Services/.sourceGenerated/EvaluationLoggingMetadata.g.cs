using SlimResolution.Core;
using SlimResolution.Core.ErrorHandling;
using SlimResolution.Core.MetadataRegistration;


namespace _UsageDemo.Services;

internal class EvaluationLoggingMetadata : ResolutionMetadataBase, IResolutionMetadata<EvaluationLogging>
{
    private Resolution<IPseudoLog> _loggerResolution { get; }


    public EvaluationLoggingMetadata(
        ResolutionSourceValidation sourceValidation,
        Resolution<IPseudoLog> loggerResolution) : base(sourceValidation)
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