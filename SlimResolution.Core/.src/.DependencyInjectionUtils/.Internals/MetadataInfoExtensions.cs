using System.Collections.Generic;


namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal static class MetadataInfoExtensions
{
    internal static void OnEach(this IEnumerable<MetadataInfo> metadataInfos,
                                HandleMetadataInfo handle)
    {
        foreach (var metadataInfo in metadataInfos) handle(metadataInfo);
    }
}