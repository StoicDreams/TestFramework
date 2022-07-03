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

	private IActions<TInstance> CallHandlerAndReturnForActAndAssertions<TInstance>(IServiceProvider serviceProvider, Action<IArrangeOptions>? setupHandler)
		where TInstance : class
	{
		#region Call setup handler from caller if defined
		setupHandler?.Invoke(new ArrangeOptions(serviceProvider));
		#endregion

		TInstance? service = serviceProvider.GetService<TInstance>();
		if (service == null)
		{
			throw new NullReferenceException($"Failed to load service {(typeof(TInstance).FullName)}");
		}
		IActions<TInstance> actions = new Actions<TInstance>(serviceProvider, service);
		return actions;
	}
}
