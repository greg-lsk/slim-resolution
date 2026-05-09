using System;


namespace SlimResolution.Core.DependencyInjectionUtils;

public delegate object InstantiateMetadata(object resolutionSource);
public delegate void RegisterMetadata(Type metadataInterfaceType, InstantiateMetadata instantiateMetadata);

public delegate object ResolveMetadataDependency(Type metadataDependencyType, object resolutionSource);

public delegate void HandleMetadataInfo(MetadataInfo metadataInfo);