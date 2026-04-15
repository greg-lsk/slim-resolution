using SlimResolution.Core;
using SlimResolution.Core.ErrorHandling;


namespace _UsageDemo.Services;

internal class EvaluationLoggingMetadata : IResolutionMetadata<EvaluationLogging>
{
    public LinkToken LinkToken { get; }
    internal Resolution<IPseudoLog> LoggerResolution { get; }


    public EvaluationLoggingMetadata(
        LinkToken token,
        Resolution<IPseudoLog> loggerResolution)
    {
        LinkToken = token;
        LoggerResolution = loggerResolution;
    }


    public EvaluationLogging Materialize(IResolutionContext context)
    {
        return new(this, context);
    }
}