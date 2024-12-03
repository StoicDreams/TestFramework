namespace StoicDreams;

public abstract partial class TestFramework
{
    public static IActions<TService> ArrangeTest<TService>(Func<IArrangeTestOptions, TService> setupHandler)
        where TService : class
    {
        IServiceCollection services = new ServiceCollection();
        return ArrangeTest(services, setupHandler);
    }

    public static IActions<TService> ArrangeTest<TService>(IServiceCollection services, Func<IArrangeTestOptions, TService> setupHandler)
        where TService : class
    {
        ArrangeTestOptions options = new(services);
        TService service = setupHandler(options);
        services.AddSingleton(service);
        IServiceProvider serviceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;
        Actions<TService> actions = new(serviceProvider, service);
        actions.SetConsoleWatch(options.ConsoleWatch);
        return actions;
    }

    public static IActions<TService> ArrangeTest<TService>(Action<IArrangeTestOptions> setupHandler)
        where TService : class
    {
        IServiceCollection services = new ServiceCollection();
        return ArrangeTest<TService>(services, setupHandler);
    }

    public static IActions<TService> ArrangeTest<TService>(IServiceCollection services, Action<IArrangeTestOptions> setupHandler)
        where TService : class
    {
        ArrangeTestOptions options = new(services);
        setupHandler(options);
        IServiceProvider serviceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;
        TService service = serviceProvider.GetServiceThrows<TService>();
        Actions<TService> actions = new(serviceProvider, service);
        actions.SetConsoleWatch(options.ConsoleWatch);
        return actions;
    }
}
