using NSubstitute.Core;
using System.Reflection;

namespace StoicDreams;

public static class Extensions
{
    /// <summary>
    /// Wrapper around IServiceProvider.GetService<T>() extension method to throw erorr on null instead of returning null values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static T GetServiceThrows<T>(this IServiceProvider serviceProvider)
    {
        T? service = serviceProvider.GetService<T>();
        if (service == null)
        {
            throw new NullReferenceException($"Failed to get expected service {(typeof(T).FullName)}");
        }
        return service;
    }

    /// <summary>
    /// Add a substitution of an interface|class for injection into test elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="setupHandler"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddSub<T>(
        this IServiceCollection services,
        Action<T>? setupHandler = null
        )
        where T : class
    {
        T sub = Substitute.For<T>();
        setupHandler?.Invoke(sub);
        services.AddSingleton(sub);
        return services;
    }

    public static TReturns RunPrivateMethod<TService, TReturns>(this TService service, string method, params object[] input)
    {
        if (service == null)
        {
            throw new NullReferenceException($"Service {(typeof(TService).FullName)} cannot be null.");
        }
        MethodInfo? methodInfo = service.GetType().GetMethod(
            method,
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.SetField | BindingFlags.SetProperty
            ) ?? throw new NullReferenceException($"Method {method} not found in service {(typeof(TService).FullName)}.");
        TReturns? result = (TReturns?)methodInfo.Invoke(service, input);
        return result == null
            ? throw new NullReferenceException($"Method {(typeof(TService).FullName)}.{method} unexpectedly returned null.")
            : result;
    }

    public static ConfiguredCall Throws<T>(this T value, Exception exception, params Func<CallInfo, T>[] returnThese)
    {
        return value.Returns(_ => throw exception);
    }
}
