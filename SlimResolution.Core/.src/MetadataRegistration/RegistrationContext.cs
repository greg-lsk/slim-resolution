using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using System.Linq;


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

            var targetType = type.GetInterfaces()
                                 .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IResolutionMetadata<>));

            if (targetType is null) continue;

            var dependencies = Dependencies(type);
            var ctor = type.GetConstructor([.. dependencies]) ?? throw new MissingMethodException("ctor not found");

            var args = dependencies
                .Select(t => t.GetGenericArguments()[0])
                .Select(t => methodInfo.MakeGenericMethod(t).CreateDelegate(typeof(Resolution<>).MakeGenericType(t)));

            action(type, targetType, () => ctor.Invoke([.. args]) ?? throw new InvalidDataException("couldn't invoke ctor"));
        }
    }

    private IEnumerable<Type> Dependencies(Type concreateType)
    {
        var binding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        var properties = concreateType.GetProperties(binding);

        for (int i = 0; i < properties.Length; i++)
        {
            if (PropertyIsResolution(properties[i]))
            {
                yield return properties[i].PropertyType;
            }
        }
    }

    private bool PropertyIsResolution(PropertyInfo info)
    {
        return info.PropertyType.IsGenericType
               && info.PropertyType.GetGenericTypeDefinition() == typeof(Resolution<>);
    }
}