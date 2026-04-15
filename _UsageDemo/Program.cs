using _UsageDemo.Services;

using SlimResolution.Core;
using SlimResolution.Extensions.MicrosoftDI;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SlimResolution.Core.ErrorHandling;


var host = Host.CreateDefaultBuilder(args) 
               .ConfigureServices((context, services) =>
               {
                   services.AddScoped<IPseudoLog, PseudoLog>();

                   services.AddSlimResolution(["_UsageDemo.dll"]);
               })
               .Build();

var i = 15;
var aspectComposer = host.Services.GetRequiredService<IComposer<EvaluationLogging>>();
//var customComposer = new CustomComposer();

var hostAspect = aspectComposer.Compose();
var throwCompose = aspectComposer.ComposeCustomly(host.Services);

hostAspect.Run(i++);

using (var scope = host.Services.CreateScope())
{
    var scopedAspect = aspectComposer.ComposeFor(scope);
    //var willThrow = customComposer.ComposeFor(scope);

    hostAspect.Run(i++);
    scopedAspect.Run(i++);
}


//internal class CustomComposer : IComposer<EvaluationLogging>
//{
//    public EvaluationLogging Compose()
//    {
//        return new();
//    }
//}

//internal class CustomMetadata : IResolutionMetadata<EvaluationLogging>
//{
//    public LinkToken LinkToken => new();

//    public EvaluationLogging Materialize(IResolutionContext context)
//    {
//        return new EvaluationLogging(this, context);
//    }
//}

internal class CustomContext : IResolutionContext
{
    public LinkToken LinkToken => new();
}

internal static class CustomComposerExtensions
{
    internal static EvaluationLogging ComposeCustomly(this IComposer<EvaluationLogging> composer ,IServiceProvider provider)
    {
        var md = provider.GetRequiredService<IResolutionMetadata<EvaluationLogging>>();

        return new(md, new CustomContext());
    }
}