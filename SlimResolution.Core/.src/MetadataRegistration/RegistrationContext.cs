using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;


namespace SlimResolution.Core.MetadataRegistration;

public class RegistrationContext
{
    public IEnumerable<MetadataInfo> MetadataInfo { get; }


    private RegistrationContext()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var dllFiles = Directory.GetFiles(baseDir, "*.dll");

        List<MetadataInfo> infos = [];

        for (int i = 0; i < dllFiles.Length; i++)
        {
            var assembly = Assembly.LoadFrom(dllFiles[i]);
            var assemblyTypes = assembly.GetTypes();

            UpdateMetadataInfo(infos, assemblyTypes);
        }

        MetadataInfo = infos;
    }
    public static RegistrationContext Instance => new();


    private void UpdateMetadataInfo(List<MetadataInfo> infos, IEnumerable<Type> types)
    {
        foreach(var type in types)
        {
            if (type.IsValueType || type.IsAbstract || type.IsInterface) continue;

            var targetType = type.GetInterfaces()
                                 .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IResolutionMetadata<>))
                                 .Select(i => i.GetGenericArguments()[0])
                                 .FirstOrDefault();

            if (targetType is null) continue;
            infos.Add(new MetadataInfo(targetType, type));
        }
    }
}