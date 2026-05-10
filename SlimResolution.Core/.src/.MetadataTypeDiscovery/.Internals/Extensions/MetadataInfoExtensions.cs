using System.Collections.Generic;


namespace SlimResolution.Core.MetadataTypeDiscovery.Internals;

internal static class MetadataInfoExtensions
{
    internal static void OnEach(this IEnumerable<RegistrationInfo> metadataInfos,
                                HandleRegistrationInfo handle)
    {
        foreach (var metadataInfo in metadataInfos) handle(metadataInfo);
    }
}