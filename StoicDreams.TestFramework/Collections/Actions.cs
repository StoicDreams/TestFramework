namespace StoicDreams;

public class Actions : IActions
{
	public Actions(object input)
	{
		Value = input;
	}

	public void Act<T>(Func<T, object> action)
	{
		Result = action((T)Value);
	}

	public void ActThrowsException(Action action)
	{
		try
		{
			action.Invoke();
		}
		catch(Exception ex)
		{
			Result = ex;
			return;
		}
		throw new Exception("Exception was expected but no exception was thrown");
	}

	public void Assert<T>(Action<T?> action)
	{
		action((T?)Result);
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

	public void ActAsync(Func<IArrangement<TInstance>, Task> action)
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

	public void AssertAsync(Func<IArrangement<TInstance>, Task> action)
	{
		action?.Invoke(Arrangement).GetAwaiter().GetResult();
	}

	private Arrangement<TInstance> Arrangement { get; }
	private TInstance Service { get; }
	private IServiceProvider ServiceProvider { get; }
}
