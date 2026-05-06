using System;
using System.Reflection;
using System.Reflection.Emit;


namespace SlimResolution.Core.ResolutionSourceProcessing;

internal class SourceAccessor
{
    private readonly AccessRootServiceProvider _accessor;

    internal AccessRootServiceProvider Accessor => _accessor;


    public SourceAccessor()
    {
        var fieldName = "_source";
        var structType = typeof(ResolutionSource);

        var field = structType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) 
            ?? throw new ArgumentException("Field not found", nameof(fieldName));
        
        if (field.FieldType != typeof(object)) throw new ArgumentException("Field type mismatch", nameof(fieldName));

        var dm = new DynamicMethod
        (
            name:           $"_get_{structType.Name}_{fieldName}",
            returnType:     typeof(object),
            parameterTypes: [structType],
            m:              typeof(SourceAccessor).Module
        );

        var il = dm.GetILGenerator();

        // For value types, we need the address of the arg on the stack to read the field.
        il.Emit(OpCodes.Ldarga_S, 0);               // load address of TStruct argument
        il.Emit(OpCodes.Ldfld, field);              // load value of the field
        il.Emit(OpCodes.Ret);

        _accessor = (AccessRootServiceProvider) dm.CreateDelegate(typeof(AccessRootServiceProvider));
    }
}