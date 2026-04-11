using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;



namespace SlimResolution.Core.MetadataRegistration;

public class RegistrationContext
{
    public static RegistrationContext Instance => new();


    public void OnHitRun(MethodInfo methodInfo, Action<Type, Type, Func<object>> action)
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var dllFiles = Directory.GetFiles(baseDir, "*.dll");

        for (int i = 0; i < dllFiles.Length; i++)
        {
            var assembly = Assembly.LoadFrom(dllFiles[i]);
            var assemblyTypes = assembly.GetTypes();

            UpdateMetadataInfo(assemblyTypes, methodInfo, action);
        }
    }


    private void UpdateMetadataInfo(IEnumerable<Type> types,
                                    MethodInfo methodInfo,
                                    Action<Type, Type, Func<object>> action)
    {
        foreach(var type in types)
        {
            if (type.IsValueType || type.IsAbstract || type.IsInterface) continue;

            var metadataInterface = TryGetMetadataIResolutionInterface(reflectedType);
            if (metadataInterface is null) continue;

            List<Type> resolutionTypes = [];
            List<Delegate> resolutionDelegates = [];
            AnalyzeMetadataProperties(reflectedType, resolutionTypes, methodInfo, resolutionDelegates);

            var dependencies = Dependencies(type);
            var ctor = type.GetConstructor([.. dependencies]) ?? throw new MissingMethodException("ctor not found");

            action(metadataInterface, () => ctor.Invoke([.. resolutionDelegates]) 
                ?? throw new InvalidDataException("couldn't invoke ctor"));
        }
    }


    private void AnalyzeMetadataProperties(Type concreteMetadata,
                                           List<Type> resolutionTypes,
                                           MethodInfo methodInfo,
                                           List<Delegate> resolutionDelegates)
    {
        var binding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        foreach(var property in concreteMetadata.GetProperties(binding))
        {
            if (PropertyIsResolution(property))
            {
                resolutionTypes.Add(property.PropertyType);
                resolutionDelegates.Add(CreateResolutionDelegate(property.PropertyType, methodInfo));
            }
        }
    }

    private Type? TryGetMetadataIResolutionInterface(Type reflectedType)
    {
        foreach(var implementedInterface in reflectedType.GetInterfaces())
        {
            if (implementedInterface.IsGenericType 
                && implementedInterface.GetGenericTypeDefinition() == typeof(IResolutionMetadata<>))
            {
                return implementedInterface;
            }
        }
        return null;
    }

    private bool PropertyIsResolution(PropertyInfo info)
    {
        return info.PropertyType.IsGenericType
               && info.PropertyType.GetGenericTypeDefinition() == typeof(Resolution<>);
    }

    private Delegate CreateResolutionDelegate(Type resolutionType, MethodInfo methodInfo)
    {
        var serviceType = resolutionType.GetGenericArguments()[0];

        return methodInfo.MakeGenericMethod(serviceType)
                         .CreateDelegate(typeof(Resolution<>)
                         .MakeGenericType(serviceType));
    }
}