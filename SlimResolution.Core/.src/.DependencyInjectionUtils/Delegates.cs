using System;


namespace SlimResolution.Core.DependencyInjectionUtils;

public delegate object ResolveService<TProvider>(TProvider provider) 
    where TProvider : notnull;

public delegate object ResolveServiceFromType<TProvider>(TProvider provider, Type serviceAbstractType)
    where TProvider : notnull;

public delegate void RegisterService<TProvider>(Type serviceAbstractType, ResolveService<TProvider> resolveService) 
    where TProvider : notnull;


public delegate void HandleMetadataInfo(RegistrationInfo metadataInfo);