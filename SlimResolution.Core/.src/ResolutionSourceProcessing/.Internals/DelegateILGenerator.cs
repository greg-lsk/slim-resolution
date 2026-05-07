using System;
using System.Reflection.Emit;


namespace SlimResolution.Core.ResolutionSourceProcessing.Internals;

internal class DelegateILGenerator : IDelegateFactory
{
    CreateResolutionSource IDelegateFactory.TryConstructCreationDelegate(IReflectionContext context)
    {
        var dynamicMethod = new DynamicMethod
        (
            name:           $"_create_{context.ResolutionSourceType.Name}",
            returnType:     context.ResolutionSourceType,
            parameterTypes: [context.WrappedType],
            m:              context.ResolutionSourceType.Module
        );

        var ilGenerator = dynamicMethod.GetILGenerator();

        var local = ilGenerator.DeclareLocal(context.ResolutionSourceType);

        ilGenerator.Emit(OpCodes.Ldloca_S, local);                       
        ilGenerator.Emit(OpCodes.Ldarg_0);                               
        ilGenerator.Emit(OpCodes.Call, context.TryGetConstructorInfo()); 
        ilGenerator.Emit(OpCodes.Ldloc_0);                               
        ilGenerator.Emit(OpCodes.Ret);

        return dynamicMethod.CreateDelegate(typeof(CreateResolutionSource)) as CreateResolutionSource
        ?? throw new InvalidCastException("Invalid cast");
    }

    AccessRootServiceProvider IDelegateFactory.TryConstructAccessDelegate(IReflectionContext context)
    {
        var dynamicMethod = new DynamicMethod
        (
            name:           $"_get_{context.ResolutionSourceType.Name}_{context.WrappedTypeFieldName}",
            returnType:     context.WrappedType,
            parameterTypes: [context.ResolutionSourceType],
            m:              context.ResolutionSourceType.Module
        );

        var ilGenerator = dynamicMethod.GetILGenerator();

        ilGenerator.Emit(OpCodes.Ldarga_S, 0);                             
        ilGenerator.Emit(OpCodes.Ldfld, context.TryGetWrappedFieldInfo()); 
        ilGenerator.Emit(OpCodes.Ret);

        return dynamicMethod.CreateDelegate(typeof(AccessRootServiceProvider)) as AccessRootServiceProvider
        ?? throw new InvalidCastException("Invalid cast");
    }
}