namespace StoicDreams;

public class ArrangeUnitOptions : IArrangeUnitOptions
{
    internal ArrangeUnitOptions(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public T GetService<T>(Action<T>? setupHandler = null)
        where T : class
    {
        T service = ServiceProvider.GetRequiredService<T>();
        setupHandler?.Invoke(service);
        return service;
    }

    private IServiceProvider ServiceProvider { get; }
}
