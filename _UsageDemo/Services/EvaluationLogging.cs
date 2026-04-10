using SlimResolution.Core;


namespace _UsageDemo.Services;

[ResolveUsing<IResolutionMetadata<EvaluationLogging>>]
internal readonly partial struct EvaluationLogging
{
    private partial IPseudoLog Logger { get; }

    public void Run(in int context)
    {
        Logger.Log($"Evaluated: {context}");
    }
}