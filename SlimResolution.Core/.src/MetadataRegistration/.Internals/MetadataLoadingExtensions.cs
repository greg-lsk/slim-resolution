using System;
using System.Collections.Generic;

using SlimResolution.Core.MetadataRegistration.Internals.Utils;


namespace SlimResolution.Core.MetadataRegistration.Internals;

internal static class MetadataLoadingExtensions
{
    internal static IEnumerable<MetadataInfo> FilterByMetadata(this IEnumerable<Type> types)
    {
        List<MetadataInfo> typesList = [];

        foreach (var type in types)
        {
            if (type.IsNotConcreteClass()) continue;

            var closedGenericInterfaceType = type.TryGetClosedGenericInterface(typeof(IResolutionMetadata<>));
            if (closedGenericInterfaceType is null) continue;

            typesList.Add(MetadataInfo.Create(closedGenericInterfaceType, type));
        }

        return typesList;
    }

    internal static void OnEach(this IEnumerable<MetadataInfo> metadataInfos,
                                MetadataInfoHandler registerDelegate)
    {
        foreach (var metadataInfo in metadataInfos) registerDelegate(metadataInfo);
    }
}