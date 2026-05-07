using System;

using SlimResolution.Core.ExtensionHelpers;
using SlimResolution.Core.ResolutionSourceProcessing.Internals;


namespace SlimResolution.Core.ResolutionSourceProcessing.DependencyInjection;

public static class ExtensionContextExtensions
{
    public static void RegisterSourceProcessingEssentials(this ExtensionContext context, Action<Type, Type> register)
    {
        register(typeof(IDelegateFactory), typeof(DelegateILGenerator));
        register(typeof(IDelegateCreator), typeof(ProcessingContext));
    }
}