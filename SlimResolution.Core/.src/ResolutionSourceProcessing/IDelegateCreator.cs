using System;


namespace SlimResolution.Core.ResolutionSourceProcessing;

public interface IDelegateCreator
{
    public TDelegate Create<TDelegate>() where TDelegate : Delegate;
}