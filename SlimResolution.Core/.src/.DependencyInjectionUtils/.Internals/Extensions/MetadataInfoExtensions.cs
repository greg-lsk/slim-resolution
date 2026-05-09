using System.Collections.Generic;


namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal static class MetadataInfoExtensions
{
    internal static void OnEach(this IEnumerable<RegistrationInfo> metadataInfos,
                                HandleMetadataInfo handle)
    {
        foreach (var metadataInfo in metadataInfos) handle(metadataInfo);
    }
}