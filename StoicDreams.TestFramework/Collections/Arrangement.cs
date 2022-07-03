namespace StoicDreams;

public class Arrangement<TInstance> : IArrangement<TInstance>
	where TInstance : class
{
	public Arrangement(IServiceProvider serviceProvider, TInstance service)
	{
		ServiceProvider = serviceProvider;
		Service = service;
	}

	public TInstance Service { get; }
	public Mock<T> GetMock<T>() where T: class => ServiceProvider.GetMock<T>();
	public T? GetResult<T>() => (T?)Result;

	internal object? Result { get; set; }

	private IServiceProvider ServiceProvider { get; }
}
