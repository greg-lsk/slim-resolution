using System;
using System.Reflection;


namespace SlimResolution.Core.ResolutionSourceProcessing.Internals;

internal class ProcessingContext : IReflectionContext, IDelegateCreator
{
    Type IReflectionContext.WrappedType => typeof(object);
    string IReflectionContext.WrappedTypeFieldName => "_source";

    Type IReflectionContext.ResolutionSourceType => typeof(ResolutionSource);

    BindingFlags IReflectionContext.BindingAttribute => BindingFlags.Instance | BindingFlags.NonPublic;


    private readonly IDelegateFactory _delegateFactory;


    public ProcessingContext(IDelegateFactory delegateFactory)
    {
        _delegateFactory = delegateFactory;
    }


    TDelegate IDelegateCreator.Create<TDelegate>() 
    {
        if (typeof(TDelegate) == typeof(CreateResolutionSource))
        {
            return (_delegateFactory.TryConstructCreationDelegate(this) as TDelegate)!;
        }

        if (typeof(TDelegate) == typeof(AccessRootServiceProvider))
        {
            return (_delegateFactory.TryConstructAccessDelegate(this) as TDelegate)!;
        }

        throw new ArgumentException();
    }


    FieldInfo IReflectionContext.TryGetWrappedFieldInfo()
    {
        var context = this as IReflectionContext;

        var fieldInfo = context.ResolutionSourceType.GetField(context.WrappedTypeFieldName, context.BindingAttribute);

        if (fieldInfo is null) 
            throw new ArgumentException("Field not found", context.WrappedTypeFieldName);

        if (fieldInfo.FieldType != context.WrappedType) 
            throw new ArgumentException("Field type mismatch", context.WrappedTypeFieldName);

        return fieldInfo;
    }

    ConstructorInfo IReflectionContext.TryGetConstructorInfo()
    {
        var context = this as IReflectionContext;

        return context.ResolutionSourceType.GetConstructor
        (
            bindingAttr: context.BindingAttribute,
            binder:      null,
            types:       [context.WrappedType],
            modifiers:   null
        ) ?? throw new InvalidOperationException("Private ctor not found.");
    }
}