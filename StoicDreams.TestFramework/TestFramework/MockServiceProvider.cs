using System.Reflection;

namespace StoicDreams;

public abstract partial class TestFramework
{
    private MockTypes MockType { get; set; }

    /// <summary>
    /// Create an instance of IServiceProvider to use explicitely for unit testing a component TClass with interface TInterface.
    /// A Singleton instance of TClass:TInterface is added as well as mocked instances of parameter dependencies of the first constructor.
    /// Call IServiceProvider.GetService<TInterface>() to fetch instance.
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TClass"></typeparam>
    /// <returns></returns>
    protected IServiceProvider MockServiceProvider<TInterface, TClass>(MockTypes mockType)
        where TInterface : class
        where TClass : class, TInterface
    {
        MockType = mockType;
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
    protected IServiceProvider MockServiceProvider<TClass>(Action<IServiceCollection>? setupHandler, MockTypes mockType)
        where TClass : class
    {
        MockType = mockType;
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
            Type paramType = info.ParameterType;
            dynamic instance = Substitute.For(new Type[] { info.ParameterType }, Array.Empty<object>());
            ServiceDescriptor mockDescriptor = new(paramType, instance);
            services.Add(mockDescriptor);
        }
    }

    private ParameterInfo[] GetParameterInfo<TClass>()
    {
        try
        {
            return typeof(TClass).GetConstructors().First().GetParameters();
        }
        catch { }
        try
        {
            return typeof(TClass).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First().GetParameters();
        }
        catch { }
        return Array.Empty<ParameterInfo>();
    }
}
