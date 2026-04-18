using System;


namespace SlimResolution.Core.MetadataRegistration;

public delegate void Registration(Type interfaceType, Func<object> concreteTypeFactory);
public delegate object Resolution(Type serviceType, object source);

public delegate void MetadataInfoHandler(in MetadataInfo metadataInfo);