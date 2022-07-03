namespace StoicDreams;

public class ArrangeTestOptions : IArrangeTestOptions
{
	public ArrangeTestOptions(IServiceCollection services)
	{
		Services = services;
	}

	public IArrangeTestOptions AddMock<TService>(Action<Mock<TService>>? setupHandler = null)
		where TService : class
	{
		Services.AddMock(setupHandler);
		return this;
	}

	public IArrangeTestOptions AddService<TService>()
		where TService : class
	{
		Services.AddSingleton<TService>();
		return this;
	}

	public IArrangeTestOptions AddService<TInterface, TService>()
		where TService : class, TInterface
		where TInterface : class
	{
		Services.AddSingleton<TInterface, TService>();
		return this;
	}

	public IArrangeTestOptions AddService<TService>(Func<TService> setupHandler)
		where TService : class
	{
		TService service = setupHandler();
		Services.AddSingleton(service);
		return this;
	}

	private IServiceCollection Services { get; }
}
