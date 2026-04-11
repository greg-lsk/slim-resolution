using System;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;


namespace SlimResolution.Core.MetadataRegistration;

public class RegistrationContext
{
    private readonly static string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

    private readonly IServiceResolver _serviceResolver;


    private RegistrationContext(IServiceResolver serviceResolver)
    {
        _serviceResolver = serviceResolver;
    }
    public static RegistrationContext Create(IServiceResolver serviceResolver)
    {
        return new(serviceResolver);
    }


    public void RegisterMetadata(Action<Type, Func<object>> action)
    {
        var dllFiles = Directory.GetFiles(_baseDirectory, "*.dll");

        foreach(var dllFile in dllFiles)
        {
            var assembly = Assembly.LoadFrom(dllFile);
            var assemblyTypes = assembly.GetTypes();

            AnalyzeAssemblyTypes(assemblyTypes, action);
        }
    }


    private void AnalyzeAssemblyTypes(IEnumerable<Type> types,
                                      Action<Type, Func<object>> action)
    {
        foreach(var reflectedType in types)
        {
            if (reflectedType.IsValueType || reflectedType.IsAbstract || reflectedType.IsInterface) continue;

            var metadataInterface = TryGetMetadataIResolutionInterface(reflectedType);
            if (metadataInterface is null) continue;

            List<Type> resolutionTypes = [];
            List<Delegate> resolutionDelegates = [];
            AnalyzeMetadataProperties(reflectedType, resolutionTypes, resolutionDelegates);
            
            var ctor = reflectedType.GetConstructor([.. resolutionTypes]) 
                ?? throw new MissingMethodException("ctor not found");
            
            action(metadataInterface, () => ctor.Invoke([.. resolutionDelegates]) 
                ?? throw new InvalidDataException("couldn't invoke ctor"));
        }
    }


    private void AnalyzeMetadataProperties(Type concreteMetadata,
                                           List<Type> resolutionTypes,
                                           List<Delegate> resolutionDelegates)
    {
        var binding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        foreach(var property in concreteMetadata.GetProperties(binding))
        {
            if (PropertyIsResolution(property))
            {
                resolutionTypes.Add(property.PropertyType);
                resolutionDelegates.Add(CreateResolutionDelegate(property.PropertyType));
            }
        }
    }

    private static Type? TryGetMetadataIResolutionInterface(Type reflectedType)
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

    private static bool PropertyIsResolution(PropertyInfo info)
    {
        return info.PropertyType.IsGenericType
               && info.PropertyType.GetGenericTypeDefinition() == typeof(Resolution<>);
    }

    private Delegate CreateResolutionDelegate(Type resolutionType)
    {
        var serviceType = resolutionType.GetGenericArguments()[0];
        var methodInfo = typeof(IServiceResolver).GetMethod(nameof(IServiceResolver.Resolve))
                                                 .MakeGenericMethod(serviceType);

        var contextParam = Expression.Parameter(typeof(IResolutionContext), "context");
        var resolverConst = Expression.Constant(_serviceResolver, typeof(IServiceResolver));

        var call = Expression.Call(resolverConst, methodInfo, contextParam);

        var lambdaType = typeof(Resolution<>).MakeGenericType(serviceType);
        var lambda = Expression.Lambda(lambdaType, call, contextParam);

        return lambda.Compile();
    }
}