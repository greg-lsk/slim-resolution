using System;
using System.IO;
using System.Reflection;

using SlimResolution.Core.MetadataRegistration.Internals;
using SlimResolution.Core.MetadataRegistration.Internals.Utils;


namespace SlimResolution.Core.MetadataRegistration;

public class RegistrationContext
{
    private readonly Registration _registration;
    private readonly Resolution _resolution;
    private readonly ResolutionDelegateBuilder _delegateBuiler;


    private RegistrationContext(Registration registration, Resolution resolution)
    {
        _resolution = resolution;
        _registration = registration;
        _delegateBuiler = ResolutionDelegateBuilder.Instance;
    }
    public static RegistrationContext Create(Registration registration, Resolution resolution)
    {
        return new(registration, resolution);
    }


    public void RegisterMetadata()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var dllFiles = Directory.GetFiles(baseDirectory, "*.dll");

        foreach(var dllFile in dllFiles)
        {
            var assembly = Assembly.LoadFrom(dllFile);

            assembly.GetTypes()
                    .FilterByMetadata()
                    .OnEach((in metadataInfo) => 
                    {
                        var binding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

                        var metadataType = metadataInfo.ConcreteType;
                        metadataType.GetProperties(binding)
                                    .RunRegistration(in metadataInfo,
                                                     _delegateBuiler,
                                                     _registration,
                                                     _resolution);
                    });
        }
    }
}