using System;


namespace SlimResolution.Core.MetadataRegistration.Internals;

internal delegate void Register(in (Type InterfaceType, Type ConcreteType) metadataInfo);