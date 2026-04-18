using System;
using Microsoft.Extensions.DependencyInjection;

using SlimResolution.Core;
using SlimResolution.Core.ExtensionHelpers;


namespace SlimResolution.Extensions.MicrosoftDI;

public static class IComposerExtensions
{
    public static TResolved ComposeFor<TResolved>(this IComposer<TResolved> composer, IServiceScope scope)
        where TResolved : struct
    {
        var source = composer.ResolutionSourceFactory(scope.ServiceProvider);

        try { return ExtensionContext.Instance.Matarialize(composer, source); }
        catch (ArgumentException) { throw; }
    }   
}