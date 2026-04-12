using System;
using System.Linq.Expressions;


namespace SlimResolution.Core.MetadataRegistration.Internals.Utils;

internal readonly struct ResolutionDelegateBuilder
{
    internal static ResolutionDelegateBuilder Instance => new();


    internal Delegate BuildDelegate(Type type, IServiceResolver resolver)
    {
        var serviceType = type.GetGenericArguments()[0];
        var methodInfo = typeof(IServiceResolver).GetMethod(nameof(IServiceResolver.Resolve))
                                                 .MakeGenericMethod(serviceType);


        var contextParam = Expression.Parameter(typeof(IResolutionContext), "context");
        var resolverConst = Expression.Constant(resolver, typeof(IServiceResolver));

        var call = Expression.Call(resolverConst, methodInfo, contextParam);

        var lambdaType = typeof(Resolution<>).MakeGenericType(serviceType);
        var lambda = Expression.Lambda(lambdaType, call, contextParam);

        return lambda.Compile();
    }
}