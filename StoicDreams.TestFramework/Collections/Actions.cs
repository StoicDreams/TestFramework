namespace StoicDreams;

public class Actions : IActions
{
	public Actions(object input)
	{
		Value = input;
	}

	public void Act(Func<object, object> action)
	{
		Result = action(Value);
	}

	public void Assert(Action<object?> action)
	{
		action(Result);
	}

	private object Value { get; }
	private object? Result { get; set; }
}

public class Actions<TInstance> : IActions<TInstance>
	where TInstance : class
{
	internal Actions(IServiceProvider serviceProvider, TInstance service)
	{
		ServiceProvider = serviceProvider;
		Service = service;
		Arrangement = new Arrangement<TInstance>(serviceProvider, service);
	}
	
	public void Act(Action<IArrangement<TInstance>> action)
	{
		try
		{
			action?.Invoke(Arrangement);
		}
		catch (Exception ex)
		{
			Arrangement.Result = ex;
			throw;
		}
	}

	public void Act(Func<IArrangement<TInstance>, object?> action)
	{
		try
		{
			Arrangement.Result = action?.Invoke(Arrangement);
		}
		catch (Exception ex)
		{
			Arrangement.Result = ex;
			throw;
		}
	}

	public void Act(Func<IArrangement<TInstance>, Task> action)
	{
		try
		{
			action?.Invoke(Arrangement).GetAwaiter().GetResult();
		}
		catch (Exception ex)
		{
			Arrangement.Result = ex;
			throw;
		}
	}


	public void Act(Func<IArrangement<TInstance>, Task<object?>> action)
	{
		try
		{
			Arrangement.Result = action?.Invoke(Arrangement).GetAwaiter().GetResult();
		}
		catch (Exception ex)
		{
			Arrangement.Result = ex;
			throw;
		}
	}

	public void Assert(Action<IArrangement<TInstance>> action)
	{
		action?.Invoke(Arrangement);
	}

	public void Assert(Func<IArrangement<TInstance>, Task> action)
	{
		action?.Invoke(Arrangement).GetAwaiter().GetResult();
	}

	private Arrangement<TInstance> Arrangement { get; }
	private TInstance Service { get; }
	private IServiceProvider ServiceProvider { get; }
}
