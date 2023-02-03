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
	protected IServiceProvider MockServiceProvider<TClass>(Action<IServiceCollection>? setupHandler)
		where TClass : class
	{
		IServiceCollection services = new ServiceCollection();
		// Add the service being unit tested
		services.AddSingleton<TClass>();
		AddDependencies<TClass>(services);
		setupHandler?.Invoke(services);
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
			if (paramType.AssemblyQualifiedName == null) { throw new NullReferenceException($"Failed to load assembly name for parameter type {paramType.FullName ?? paramType.Name}"); }
			Type? assemblyType = Type.GetType(paramType.AssemblyQualifiedName);
			if (assemblyType == null) { throw new NullReferenceException($"Failed to load assembly type for parameter type {paramType.FullName ?? paramType.Name}"); }
			Type[] typeArgs = { assemblyType };
			Type repositoryType = mockType.MakeGenericType(typeArgs);
			dynamic? instance = Activator.CreateInstance(repositoryType);
			if (instance == null) { throw new NullReferenceException($"Failed to create a mocked instance for {repositoryType.FullName ?? repositoryType.Name}"); }
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
