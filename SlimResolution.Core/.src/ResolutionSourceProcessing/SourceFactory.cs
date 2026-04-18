using System;
using System.Reflection;
using System.Reflection.Emit;


namespace SlimResolution.Core.ResolutionSourceProcessing;

internal class SourceFactory
{
    private readonly Func<object, ResolutionSource> _factory;

    internal Func<object, ResolutionSource> Factory => _factory;


    internal SourceFactory()
    {
        var structType = typeof(ResolutionSource);

        var ctor = structType.GetConstructor
        (
            bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic,
            binder:      null,
            types:       [typeof(object)],
            modifiers:   null
        ) ?? throw new InvalidOperationException("Private ctor not found.");
        
        var dm = new DynamicMethod
        (
            name:           $"_create_{structType.Name}",
            returnType:     structType,
            parameterTypes: [typeof(object)],
            m:              typeof(SourceFactory).Module
        );

        var il = dm.GetILGenerator();

        // For value types, we need to emit a local, initialize it, call the ctor on its address, then load it.
        var local = il.DeclareLocal(structType);

        il.Emit(OpCodes.Ldloca_S, local);         // load address of local (byref)
        il.Emit(OpCodes.Ldarg_0);                 // load object arg
        il.Emit(OpCodes.Call, ctor);              // call instance ctor (ctor expects 'this' byref on value types)
        il.Emit(OpCodes.Ldloc_0);                 // load initialized struct
        il.Emit(OpCodes.Ret);

        _factory = (Func<object?, ResolutionSource>)dm.CreateDelegate(typeof(Func<object?, ResolutionSource>));
    }
}