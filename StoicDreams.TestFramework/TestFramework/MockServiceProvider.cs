using System.Reflection;

namespace StoicDreams;

public abstract partial class TestFramework
{
	/// <summary>
	/// Create an instance of IServiceProvider to use explicitely for unit testing a component TClass with interface TInterface.
	/// A Singleton instance of TClass:TInterface is added as well as mocked instances of parameter dependencies of the first constructor.
	/// Call IServiceProvider.GetService<TInterface>() to fetch instance.
	/// </summary>
	/// <typeparam name="TInterface"></typeparam>
	/// <typeparam name="TClass"></typeparam>
	/// <returns></returns>
	protected IServiceProvider MockServiceProvider<TInterface, TClass>()
		where TInterface : class
		where TClass : class, TInterface
	{
		IServiceCollection services = new ServiceCollection();
		// Add the service being unit tested
		services.AddSingleton<TInterface, TClass>();
		AddDependencies<TClass>(services);
		return services.BuildServiceProvider();
	}

	/// <summary>
	/// Create an instance of IServiceProvider to use explicitely for unit testing a component TClass.
	/// A Singleton instance of TClass is added as well as mocked instances of parameter dependencies of the first constructor.
	/// Call IServiceProvider.GetService<TClass>() to fetch instance.
	/// </summary>
	/// <typeparam name="TClass"></typeparam>
	/// <returns></returns>
	protected IServiceProvider MockServiceProvider<TClass>()
		where TClass : class
	{
		IServiceCollection services = new ServiceCollection();
		// Add the service being unit tested
		services.AddSingleton<TClass>();
		AddDependencies<TClass>(services);
		return services.BuildServiceProvider();
	}

	private void AddDependencies<TClass>(IServiceCollection services)
	{
		ParameterInfo[] infoArray = GetParameterInfo<TClass>();
		foreach (ParameterInfo info in infoArray)
		{
			#region Create instance of Mock<T> to use for services
			Type mockType = typeof(Mock<>);
			Type paramType = info.ParameterType;
			Type[] typeArgs = { Type.GetType(info.ParameterType.AssemblyQualifiedName) };
			Type repositoryType = mockType.MakeGenericType(typeArgs);
			dynamic instance = Activator.CreateInstance(repositoryType);
			#endregion

			#region Add mocked interface|class instance which will be injected into constructor
			ServiceDescriptor serviceDescriptor = new ServiceDescriptor(paramType, instance.Object);
			services.Add(serviceDescriptor);
			#endregion

			#region Add Mock<T> instance so test setup can fetch for additional configuration
			ServiceDescriptor mockDescriptor = new ServiceDescriptor(repositoryType, instance);
			services.Add(mockDescriptor);
			#endregion
		}
	}

	private ParameterInfo[] GetParameterInfo<TClass>()
	{
		try
		{
			return typeof(TClass).GetConstructors().First().GetParameters();
		}
		catch {}
		try
		{
			return typeof(TClass).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First().GetParameters();
		}
		catch {}
		return Array.Empty<ParameterInfo>();
	}
}
