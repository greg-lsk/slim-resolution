using SlimResolution.Core;
using SlimResolution.Core.Metadata;
using SlimResolution.Core.ErrorHandling;
using SlimResolution.Core.ResolutionSourceProcessing;
using SlimResolution.Core.ServiceProviderAbstractions;


namespace _UsageDemo.Services;

internal class EvaluationLoggingMetadata : ResolutionMetadataBase, IResolutionMetadata<EvaluationLogging>
{
    private Resolution<IPseudoLog> _loggerResolution { get; }


    public EvaluationLoggingMetadata(IDelegateCreator delegateCreator,
                                     Resolution<IPseudoLog> loggerResolution) : base(delegateCreator)
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