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
	/// Shortcut method for getting Mock<T> instance from IServiceProvider.
	/// Throws NullReferenceException if instance is not found.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="serviceProvider"></param>
	/// <returns></returns>
	public static Mock<T> GetMock<T>(this IServiceProvider serviceProvider)
		where T : class
	{
		return serviceProvider.GetServiceThrows<Mock<T>>();
	}

	/// <summary>
	/// Mock an instance of an interface|class for injection into tested components.
	/// Also adds a singleton instance of Mock<T> which can be retrieved from IServiceProvider.GetMock<T> extension method.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="services"></param>
	/// <param name="setupHandler"></param>
	/// <param name="lifetime"></param>
	/// <returns></returns>
	public static IServiceCollection AddMock<T>(
		this IServiceCollection services,
		Action<Mock<T>>? setupHandler = null,
		ServiceLifetime lifetime = ServiceLifetime.Singleton
		)
		where T : class
	{
		Mock<T> mock = new();
		setupHandler?.Invoke(mock);
		services.AddSingleton(mock);
		switch (lifetime)
		{
			case ServiceLifetime.Singleton:
				services.AddSingleton(mock.Object);
				break;
			case ServiceLifetime.Transient:
				services.AddTransient(_ => mock.Object);
				break;
			case ServiceLifetime.Scoped:
				services.AddScoped(_ => mock.Object);
				break;
		}
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
			BindingFlags.NonPublic | BindingFlags.Instance
			);
		if (methodInfo == null)
		{
			throw new NullReferenceException($"Method {method} not found in service {(typeof(TService).FullName)}.");
		}
		TReturns? result = (TReturns?)methodInfo.Invoke(service, input);
		if (result == null)
		{
			throw new NullReferenceException($"Method {(typeof(TService).FullName)}.{method} unexpectedly returned null.");
		}
		return result;
	}
}
