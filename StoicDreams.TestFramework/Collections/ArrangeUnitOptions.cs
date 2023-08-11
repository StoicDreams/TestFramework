using Moq;

namespace StoicDreams;

public class ArrangeUnitOptions : IArrangeUnitOptions
{
    internal ArrangeUnitOptions(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public Mock<T> GetMock<T>(Action<Mock<T>>? setupHandler = null)
        where T : class
    {
        Mock<T> mock = ServiceProvider.GetMock<T>();
        setupHandler?.Invoke(mock);
        return mock;
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
