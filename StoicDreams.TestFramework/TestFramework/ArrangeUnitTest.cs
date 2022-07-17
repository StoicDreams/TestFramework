namespace StoicDreams;

public abstract partial class TestFramework
{
	/// <summary>
	/// Arrange a Unit Test given it's class type but returning it's interface.
	/// Constructor dependencies will be automaticaly mocked.
	/// Using this setup method helps assert in tests methods being tested are accessible at the interface level.
	/// </summary>
	/// <typeparam name="TInterface">Interface of class being tested</typeparam>
	/// <typeparam name="TClass">Class being tested</typeparam>
	/// <param name="setupHandler"></param>
	/// <returns></returns>
	protected IActions<TInterface> ArrangeUnitTest<TInterface, TClass>(
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
	protected IActions<TClass> ArrangeUnitTest<TClass>(
		Action<IArrangeUnitOptions>? setupHandler = null
		)
		where TClass : class
	{
		IServiceProvider serviceProvider = MockServiceProvider<TClass>();
		return CallHandlerAndReturnForActAndAssertions<TClass>(serviceProvider, setupHandler);
	}

	protected IActions ArrangeUnitTest(
		Func<object> setupHandler
		)
	{
		return new Actions(setupHandler.Invoke());
	}

	private IActions<TService> CallHandlerAndReturnForActAndAssertions<TService>(IServiceProvider serviceProvider, Action<IArrangeUnitOptions>? setupHandler)
		where TService : class
	{
		#region Call setup handler from caller if defined
		setupHandler?.Invoke(new ArrangeUnitOptions(serviceProvider));
		#endregion

		TService? service = serviceProvider.GetService<TService>();
		if (service == null)
		{
			throw new NullReferenceException($"Failed to load service {(typeof(TService).FullName)}");
		}
		IActions<TService> actions = new Actions<TService>(serviceProvider, service);
		return actions;
	}
}
