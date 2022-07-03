namespace StoicDreams;

public abstract partial class TestFramework
{
	public IActions<TService> ArrangeTest<TService>(Func<IArrangeTestOptions, TService> setupHandler)
		where TService : class
	{
		IServiceCollection services = new ServiceCollection();
		IArrangeTestOptions options = new ArrangeTestOptions(services);
		TService service = setupHandler(options);
		services.AddSingleton(service);
		IServiceProvider serviceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;
		IActions<TService> actions = new Actions<TService>(serviceProvider, service);
		return actions;
	}

	public IActions<TService> ArrangeTest<TService>(Action<IArrangeTestOptions> setupHandler)
		where TService : class
	{
		IServiceCollection services = new ServiceCollection();
		IArrangeTestOptions options = new ArrangeTestOptions(services);
		setupHandler(options);
		IServiceProvider serviceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;
		TService service = serviceProvider.GetServiceThrows<TService>();
		IActions<TService> actions = new Actions<TService>(serviceProvider, service);
		return actions;
	}
}
