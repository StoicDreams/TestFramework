namespace StoicDreams;

public class ArrangeTestOptions : IArrangeTestOptions
{
    public ArrangeTestOptions(IServiceCollection services)
    {
        Services = services;
    }

    public IArrangeTestOptions AddSub<TService>(Action<TService>? setupHandler = null)
        where TService : class
    {
        Services.AddSub(setupHandler);
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

    public IArrangeTestOptions AddService<TService>(Func<TService, TService> setupHandler)
        where TService : class
    {
        TService service = Substitute.For<TService>();
        setupHandler.Invoke(service);
        return this;
    }

    public IArrangeTestOptions WatchConsole(params string[] messages)
    {
        ConsoleWatch = messages;
        return this;
    }

    internal string[] ConsoleWatch { get; set; } = [];
    private IServiceCollection Services { get; }
}
