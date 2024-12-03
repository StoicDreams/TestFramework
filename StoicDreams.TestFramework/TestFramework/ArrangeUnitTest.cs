namespace StoicDreams;

public abstract partial class TestFramework
{
    protected static IActions<TClass> ArrangeUnitTest<TClass>(
        Action<IArrangeUnitOptions>? setupHandler = null
        )
        where TClass : class
    {
        IServiceProvider serviceProvider = MockServiceProvider<TClass>(null);
        return CallHandlerAndReturnForActAndAssertions<TClass>(serviceProvider, setupHandler);
    }

    /// <summary>
    /// Arrange a Unit Test given it's class type but returning it's interface.
    /// Constructor dependencies will be automatically mocked.
    /// Using this setup method helps assert in tests methods being tested are accessible at the interface level.
    /// </summary>
    /// <typeparam name="TInterface">Interface of class being tested</typeparam>
    /// <typeparam name="TClass">Class being tested</typeparam>
    /// <param name="setupHandler"></param>
    /// <returns></returns>
    protected static IActions<TInterface> ArrangeUnitTest<TInterface, TClass>(
        Action<IArrangeUnitOptions>? setupHandler = null
        )
        where TClass : class, TInterface
        where TInterface : class
    {
        IServiceProvider serviceProvider = MockServiceProvider<TInterface, TClass>();
        return CallHandlerAndReturnForActAndAssertions<TInterface>(serviceProvider, setupHandler);
    }

    /// <summary>
    /// Arrange a Unit Test given the class to return for testing.
    /// Constructor dependencies will be automatically mocked.
    /// </summary>
    /// <typeparam name="TClass">Class being tested</typeparam>
    /// <param name="setupHandler"></param>
    /// <returns></returns>
    protected static IActions<TClass> ArrangeUnitTest<TClass>(
        Action<IArrangeUnitOptions>? setupHandler,
        Action<IServiceCollection>? setupServices
        )
        where TClass : class
    {
        IServiceProvider serviceProvider = MockServiceProvider<TClass>(setupServices);
        return CallHandlerAndReturnForActAndAssertions<TClass>(serviceProvider, setupHandler);
    }

    protected static IActions ArrangeUnitTest(object? input = null)
    {
        return new Actions(input ?? new object());
    }

    protected static IActions ArrangeUnitTest(
        Func<object> setupHandler
        )
    {
        return new Actions(setupHandler.Invoke());
    }

    private static IActions<TService> CallHandlerAndReturnForActAndAssertions<TService>(IServiceProvider serviceProvider, Action<IArrangeUnitOptions>? setupHandler)
        where TService : class
    {
        #region Call setup handler from caller if defined
        ArrangeUnitOptions arrangement = new(serviceProvider);
        setupHandler?.Invoke(arrangement);
        #endregion

        TService? service = serviceProvider.GetService<TService>() ?? throw new NullReferenceException($"Failed to load service {(typeof(TService).FullName)}");
        Actions<TService> actions = new(serviceProvider, service);
        actions.SetConsoleWatch(arrangement.ConsoleWatch);
        return actions;
    }
}
