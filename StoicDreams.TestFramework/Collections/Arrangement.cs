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

    public T GetResult<T>()
    {
        if (Result == null)
        {
            throw new NullReferenceException($"Expecting result of type {typeof(T).FullName} but null was returned.");
        }
        return (T)Result;
    }
    public T? GetNullableResult<T>() => (T?)Result;

    public T GetService<T>()
    {
        return ServiceProvider.GetService<T>() ?? throw new NotImplementedException($"Get service failed to load {typeof(T).Name}.");
    }

    internal object? Result { get; set; }

    private IServiceProvider ServiceProvider { get; }
}
