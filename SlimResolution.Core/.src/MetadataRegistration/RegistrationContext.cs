using System;
using System.IO;
using System.Reflection;

using SlimResolution.Core.MetadataRegistration.Internals;


namespace SlimResolution.Core.MetadataRegistration;

public class RegistrationContext
{
    private readonly IServiceResolver _propertyResolver;
    private readonly Registration _registration;

    private RegistrationContext(IServiceResolver propertyResolver, Registration registration)
    {
        _propertyResolver = propertyResolver;
        _registration = registration;
    }
    public static RegistrationContext Create(IServiceResolver propertyResolver, Registration registration)
    {
        return new(propertyResolver, registration);
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

                        metadataInfo.ConcreteType.GetProperties(binding)
                                                 .RunRegistration(in metadataInfo, _registration, _propertyResolver);

                    });
        }
    }
}