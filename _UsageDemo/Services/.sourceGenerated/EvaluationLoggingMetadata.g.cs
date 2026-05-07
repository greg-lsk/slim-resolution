using SlimResolution.Core;
using SlimResolution.Core.ErrorHandling;
using SlimResolution.Core.MetadataRegistration;
using SlimResolution.Core.ResolutionSourceProcessing;


namespace _UsageDemo.Services;

internal class EvaluationLoggingMetadata : ResolutionMetadataBase, IResolutionMetadata<EvaluationLogging>
{
    private Resolution<IPseudoLog> _loggerResolution { get; }


    public EvaluationLoggingMetadata(ResolutionSourceValidation sourceValidation,
                                     IDelegateCreator delegateCreator,
                                     Resolution<IPseudoLog> loggerResolution) : base(sourceValidation, delegateCreator)
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