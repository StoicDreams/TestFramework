namespace StoicDreams;

public abstract partial class TestFramework
{
	protected IActions<TInterface> ArrangeUnitTest<TInterface, TClass>(
		Action<IArrangeOptions>? setupHandler = null
		)
		where TClass : class, TInterface
		where TInterface : class
	{
		IServiceProvider serviceProvider = MockServiceProvider<TInterface, TClass>();
		return CallHandlerAndReturnForActAndAssertions<TInterface>(serviceProvider, setupHandler);
	}

	protected IActions<TClass> ArrangeUnitTest<TClass>(
		Action<IArrangeOptions>? setupHandler = null
		)
		where TClass : class
	{
		IServiceProvider serviceProvider = MockServiceProvider<TClass>();
		return CallHandlerAndReturnForActAndAssertions<TClass>(serviceProvider, setupHandler);
	}

	private IActions<TService> CallHandlerAndReturnForActAndAssertions<TService>(IServiceProvider serviceProvider, Action<IArrangeOptions>? setupHandler)
		where TService : class
	{
		#region Call setup handler from caller if defined
		setupHandler?.Invoke(new ArrangeOptions(serviceProvider));
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
