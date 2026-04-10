using Microsoft.Extensions.DependencyInjection;

using SlimResolution.Core;
using SlimResolution.Core.ExtensionHelpers;
using SlimResolution.Extensions.MicrosoftDI.Internals;


namespace SlimResolution.Extensions.MicrosoftDI;

public static class IComposerExtensions
{
    public static TResolved ComposeFor<TResolved>(this IComposer<TResolved> composer, IServiceScope scope)
        where TResolved : struct
    {
        var context = ResolutionContext.Create(() => scope.ServiceProvider);

        return ExtensionContext.Instance.Matarialize(composer, context);
    }
}