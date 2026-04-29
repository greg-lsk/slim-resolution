using System;


namespace SlimResolution.Core.MetadataRegistration;

public delegate object ServiceFactory();
public delegate void Registration(Type interfaceType, ServiceFactory concreteTypeFactory);

public delegate object Resolution(Type serviceType, object source);
public delegate bool ResolutionSourceValidation(object source);

public delegate void MetadataInfoHandler(in MetadataInfo metadataInfo);