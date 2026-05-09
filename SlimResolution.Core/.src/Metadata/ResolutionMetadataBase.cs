using SlimResolution.Core.ResolutionSourceProcessing;


namespace SlimResolution.Core.Metadata;

public abstract class ResolutionMetadataBase
{
   protected AccessRootServiceProvider AccessSource { get; }


    protected ResolutionMetadataBase(IDelegateCreator delegateCreator)
    {
        AccessSource = delegateCreator.Create<AccessRootServiceProvider>();
    }
}