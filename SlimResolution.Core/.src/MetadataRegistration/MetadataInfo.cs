using System;
using System.Reflection;
using System.Collections.Generic;


namespace SlimResolution.Core.MetadataRegistration;

public class MetadataInfo
{
    public Type TargetType { get; }
    public Type MetadataType { get; }

    public IEnumerable<Type> DependencyTypes
    {
        get
        {
            var binding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var properties = MetadataType.GetProperties(binding);

            for (int i = 0; i < properties.Length; i++)
            {
                if (PropertyIsResolution(properties[i])) 
                    yield return GetResolutionGenericType(properties[i]);
            }
        }
    }


    public MetadataInfo(Type targetType, Type metadataType)
    {
        TargetType = targetType;
        MetadataType = metadataType;
    }



    private bool PropertyIsResolution(PropertyInfo info)
    {
        return info.PropertyType.IsGenericType
               && info.PropertyType.GetGenericTypeDefinition() == typeof(Resolution<>);
    }

    private Type GetResolutionGenericType(PropertyInfo info)
    {
        return info.PropertyType.GetGenericArguments()[0];
    }
}