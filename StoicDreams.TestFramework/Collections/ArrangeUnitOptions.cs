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

    public IArrangeUnitOptions WatchConsole(params string[] messages)
    {
        ConsoleWatch = messages;
        return this;
    }

    internal string[] ConsoleWatch { get; set; } = [];

    private IServiceProvider ServiceProvider { get; }
}
