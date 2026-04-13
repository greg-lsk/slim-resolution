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

var hostAspect = aspectComposer.Compose();
hostAspect.Run(i++);

using (var scope = host.Services.CreateScope())
{
    var scopedAspect = aspectComposer.ComposeFor(scope);

    hostAspect.Run(i++);
    scopedAspect.Run(i++);
}