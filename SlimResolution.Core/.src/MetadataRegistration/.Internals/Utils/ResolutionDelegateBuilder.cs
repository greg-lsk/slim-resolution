using System;
using System.Linq.Expressions;


namespace SlimResolution.Core.MetadataRegistration.Internals.Utils;

internal readonly struct ResolutionDelegateBuilder
{
    internal static ResolutionDelegateBuilder Instance => new();


    internal Delegate BuildDelegate(Type type, Resolution resolutionDelegate)
    {
        var serviceType = type.GetGenericArguments()[0];

        var contextParam = Expression.Parameter(typeof(IResolutionContext), "context");

        var serviceTypeConst = Expression.Constant(serviceType, typeof(Type));
        var resolutionConst = Expression.Constant(resolutionDelegate, typeof(Resolution));


        var invokeResolution = Expression.Call
        (
            resolutionConst,
            typeof(Resolution).GetMethod(nameof(Resolution.Invoke)), 
            serviceTypeConst, 
            contextParam
        );

        var converted = Expression.Convert(invokeResolution, serviceType);

        var lambdaType = typeof(Resolution<>).MakeGenericType(serviceType);
        var lambda = Expression.Lambda(lambdaType, converted, contextParam);

        return lambda.Compile();
    }
}