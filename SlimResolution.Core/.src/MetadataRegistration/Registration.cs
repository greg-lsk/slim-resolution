using System;


namespace SlimResolution.Core.MetadataRegistration;

public delegate void Registration(Type interfaceType, Func<object> concreteTypeFactory);