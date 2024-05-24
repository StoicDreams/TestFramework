namespace StoicDreams;

public abstract partial class TestFramework
{
    public static IActions<TService> ArrangeIntegrationTest<TService>(
        Action<IArrangeIntegrationOptions>? setupHandler = null,
        params Func<IServiceCollection, IServiceCollection>[] startupHandlers
        )
        where TService : class
    {
        IServiceCollection services = new ServiceCollection();
        return ArrangeIntegrationTest<TService>(services, setupHandler, startupHandlers);
    }

    public static IActions<TService> ArrangeIntegrationTest<TService>(
        IServiceCollection services,
        Action<IArrangeIntegrationOptions>? setupHandler = null,
        params Func<IServiceCollection, IServiceCollection>[] startupHandlers
        )
        where TService : class
    {
        foreach (Func<IServiceCollection, IServiceCollection>? startupHandler in startupHandlers)
        {
            startupHandler.Invoke(services);
        }
        ArrangeIntegrationOptions options = new(services);
        setupHandler?.Invoke(options);

        IServiceProvider serviceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;
        TService? service = serviceProvider.GetService<TService>() ?? throw new NullReferenceException($"Failed to load service {(typeof(TService).FullName)}");
        IActions<TService> actions = new Actions<TService>(serviceProvider, service);
        return actions;
    }
}
