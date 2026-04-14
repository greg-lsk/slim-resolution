using _UsageDemo.Services;

using SlimResolution.Core;
using SlimResolution.Extensions.MicrosoftDI;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;


var host = Host.CreateDefaultBuilder(args) 
               .ConfigureServices((context, services) =>
               {
                   services.AddScoped<IPseudoLog, PseudoLog>();

                   services.AddSlimResolution(["_UsageDemo.dll"]);
               })
               .Build();

var i = 15;
var aspectComposer = host.Services.GetRequiredService<IComposer<EvaluationLogging>>();
var customComposer = new CustomComposer();

var customMetadata = new CustomMetadata();
//var willThrowLog = customMetadata.Materialize(new CustomContext());

var hostAspect = aspectComposer.Compose();
hostAspect.Run(i++);

using (var scope = host.Services.CreateScope())
{
    var scopedAspect = aspectComposer.ComposeFor(scope);
    //var willThrow = customComposer.ComposeFor(scope);

    hostAspect.Run(i++);
    scopedAspect.Run(i++);
}


internal class CustomComposer : IComposer<EvaluationLogging>
{
    public EvaluationLogging Compose()
    {
        return new();
    }
}

internal class CustomMetadata : IResolutionMetadata<EvaluationLogging>
{
    public EvaluationLogging Materialize(IResolutionContext context)
    {
        return new EvaluationLogging(this,  context);
    }
}

internal class CustomContext : IResolutionContext;