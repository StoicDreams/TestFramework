namespace StoicDreams;

public class ArrangeUnitOptions : IArrangeUnitOptions
{
    internal ArrangeUnitOptions(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public T GetMock<T>(Action<T>? setupHandler = null)
        where T : class
    {
        T mock = ServiceProvider.GetMock<T>();
        setupHandler?.Invoke(mock);
        return mock;
    }

    private IServiceProvider ServiceProvider { get; }
}
