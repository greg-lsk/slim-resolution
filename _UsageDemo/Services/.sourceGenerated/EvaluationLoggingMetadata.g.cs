using SlimResolution.Core;


namespace _UsageDemo.Services;

internal class EvaluationLoggingMetadata : IResolutionMetadata<EvaluationLogging>
{
    internal Resolution<IPseudoLog> LoggerResolution { get; }


    internal EvaluationLoggingMetadata(
        Resolution<IPseudoLog> loggerResolution)
    {
        LoggerResolution = loggerResolution;
    }


    public EvaluationLogging Materialize(IResolutionContext context)
    {
        return new(this, context);
    }
}