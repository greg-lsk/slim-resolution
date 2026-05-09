using System;
using System.Linq.Expressions;


namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal readonly struct ResolutionDelegateBuilder<TProvider> where TProvider : notnull
{
    internal static ResolutionDelegateBuilder<TProvider> Instance => new();


    internal Delegate BuildDelegate(Type type, ResolveServiceFromType<TProvider> resolveFromType)
    {
        var serviceType = type.GetGenericArguments()[0];

        var providerParam = Expression.Parameter(typeof(object), "source");
        var providerAsTProvider = Expression.Convert(providerParam, typeof(TProvider));

        var serviceTypeConst = Expression.Constant(serviceType, typeof(Type));
        var resolutionConst = Expression.Constant(resolveFromType, typeof(ResolveServiceFromType<TProvider>));


        var invokeResolution = Expression.Call
        (
            resolutionConst,
            typeof(ResolveServiceFromType<TProvider>).GetMethod(nameof(ResolveServiceFromType<>.Invoke)),
            providerAsTProvider,
            serviceTypeConst
        );

        var resolvedServiceCast = Expression.Convert(invokeResolution, serviceType);


        var lambdaType = typeof(Resolution<>).MakeGenericType(serviceType);
        var lambda = Expression.Lambda(lambdaType, resolvedServiceCast, providerParam);

        return lambda.Compile();
    }
}