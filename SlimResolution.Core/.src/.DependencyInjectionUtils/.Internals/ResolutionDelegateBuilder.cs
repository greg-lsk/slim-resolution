using System;
using System.Linq.Expressions;


namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal readonly struct ResolutionDelegateBuilder
{
    internal static ResolutionDelegateBuilder Instance => new();


    internal Delegate BuildDelegate(Type type, ResolveMetadataDependency resolveMetadataDependency)
    {
        var serviceType = type.GetGenericArguments()[0];

        var sourceParam = Expression.Parameter(typeof(object), "source");

        var serviceTypeConst = Expression.Constant(serviceType, typeof(Type));
        var resolutionConst = Expression.Constant(resolveMetadataDependency, typeof(ResolveMetadataDependency));


        var invokeResolution = Expression.Call
        (
            resolutionConst,
            typeof(ResolveMetadataDependency).GetMethod(nameof(ResolveMetadataDependency.Invoke)), 
            serviceTypeConst, 
            sourceParam
        );

        var converted = Expression.Convert(invokeResolution, serviceType);

        var lambdaType = typeof(Resolution<>).MakeGenericType(serviceType);
        var lambda = Expression.Lambda(lambdaType, converted, sourceParam);

        return lambda.Compile();
    } 
}