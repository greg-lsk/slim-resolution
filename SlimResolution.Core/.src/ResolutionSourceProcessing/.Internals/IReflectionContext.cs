using System;
using System.Reflection;


namespace SlimResolution.Core.ResolutionSourceProcessing.Internals;

internal interface IReflectionContext
{
    internal Type WrappedType { get; }
    internal string WrappedTypeFieldName { get; }

    internal Type ResolutionSourceType { get; }

    internal BindingFlags BindingAttribute { get; }


    internal FieldInfo TryGetWrappedFieldInfo();
    internal ConstructorInfo TryGetConstructorInfo();
}