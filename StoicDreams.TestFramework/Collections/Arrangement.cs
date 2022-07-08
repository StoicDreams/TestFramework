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
	public T GetResult<T>()
	{
		if (Result == null)
		{
			throw new NullReferenceException($"Expecting result of type {typeof(T).FullName} but null was returned.");
		}
		return (T)Result;
	}
	public T? GetNullableResult<T>() => (T?)Result;

	internal object? Result { get; set; }

	private IServiceProvider ServiceProvider { get; }
}
