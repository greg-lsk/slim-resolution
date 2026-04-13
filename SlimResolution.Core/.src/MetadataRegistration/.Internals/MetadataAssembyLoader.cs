using System;
using System.IO;
using System.Reflection;


namespace SlimResolution.Core.MetadataRegistration.Internals;

internal class MetadataAssembyLoader : MetadataLoader
{
    private readonly string[] _dllFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "_UsageDemo.dll");

    public override void OnEach(MetadataInfoHandler handler)
    {
        foreach (var dllFile in _dllFiles)
        {
            var assembly = Assembly.LoadFrom(dllFile);

            assembly.GetTypes()
                    .FilterByMetadata()
                    .OnEach(handler);
        }
    }
}