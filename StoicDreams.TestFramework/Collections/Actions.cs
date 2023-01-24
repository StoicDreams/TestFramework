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

	public void Act<T>(Func<T, Task<object>> action)
	{
		Result = action((T)Value).GetAwaiter().GetResult();
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

	public void ActThrowsException(Func<Task> action)
	{
		try
		{
			action.Invoke().GetAwaiter().GetResult();
		}
		catch (Exception ex)
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
		action?.Invoke(Arrangement);
	}

	public void Act(Func<IArrangement<TInstance>, object?> action)
	{
		Arrangement.Result = action?.Invoke(Arrangement);
	}

	public void Act(Func<IArrangement<TInstance>, Task> action)
	{
		action?.Invoke(Arrangement).GetAwaiter().GetResult();
	}


	public void Act(Func<IArrangement<TInstance>, Task<object?>> action)
	{
		Arrangement.Result = action?.Invoke(Arrangement).GetAwaiter().GetResult();
	}

	public void ActThrowsException(Action<IArrangement<TInstance>> action)
	{
		try
		{
			action.Invoke(Arrangement);
		}
		catch (Exception ex)
		{
			Arrangement.Result = ex;
			return;
		}
		throw new Exception("Exception was expected but no exception was thrown");
	}

	public void ActThrowsException(Func<IArrangement<TInstance>, Task> action)
	{
		try
		{
			action.Invoke(Arrangement).GetAwaiter().GetResult();
		}
		catch (Exception ex)
		{
			Arrangement.Result = ex;
			return;
		}
		throw new Exception("Exception was expected but no exception was thrown");
	}

	public void ActThrowsException<TException>(Action<IArrangement<TInstance>> action)
		where TException : Exception
	{
		try
		{
			action.Invoke(Arrangement);
		}
		catch (TException ex)
		{
			Arrangement.Result = ex;
			return;
		}
		throw new Exception("Exception was expected but no exception was thrown");
	}

	public void ActThrowsException<TException>(Func<IArrangement<TInstance>, Task> action)
		where TException : Exception
	{
		try
		{
			action.Invoke(Arrangement).GetAwaiter().GetResult();
		}
		catch (TException ex)
		{
			Arrangement.Result = ex;
			return;
		}
		throw new Exception("Exception was expected but no exception was thrown");
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
