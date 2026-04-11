using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

using SlimResolution.Core;
using SlimResolution.Core.ExtensionHelpers;
using SlimResolution.Core.MetadataRegistration;
using SlimResolution.Extensions.MicrosoftDI.Internals;


namespace SlimResolution.Extensions.MicrosoftDI;

public static class Registration
{
    public static IServiceCollection AddSlimResolution(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IComposer<>), ExtensionContext.Instance.GetComposerType())
                .AddSingleton<IResolutionContext, ResolutionContext>(provider =>
                {
                    return new(() => provider);
                });

        var registrationContext = RegistrationContext.Instance;
        registrationContext.RegisterMetadata
        (
            typeof(RegistrationHelper).GetMethod
            (
                nameof(RegistrationHelper.GenericResolution),
                BindingFlags.Public | BindingFlags.Static
            ),

            (i, f) => services.AddSingleton(i, provider => f())
        );

        return services;
    }
}