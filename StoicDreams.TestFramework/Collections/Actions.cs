namespace StoicDreams;

public class Actions : IActions
{
    public Actions(object input)
    {
        Value = input;
    }

    public IActions Act<T>(Func<T, object> action)
    {
        Result = action((T)Value);
        return this;
    }

    public IActions Act<T>(Func<T, Task<object>> action)
    {
        Result = action((T)Value).GetAwaiter().GetResult();
        return this;
    }

    public IActions ActThrowsException(Action action)
    {
        try
        {
            action.Invoke();
        }
        catch (Exception ex)
        {
            Result = ex;
            return this;
        }
        throw new Exception("Exception was expected but no exception was thrown");
    }

    public IActions ActThrowsException(Func<Task> action)
    {
        try
        {
            action.Invoke().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Result = ex;
            return this;
        }
        throw new Exception("Exception was expected but no exception was thrown");
    }

    public IActions ActThrowsException<TException>(Action action)
        where TException : Exception
    {
        try
        {
            action.Invoke();
        }
        catch (TException ex)
        {
            Result = ex;
            return this;
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception of type {typeof(TException).FullName} was expected but {ex.GetType().FullName} was thrown instead.", ex);
        }
        throw new Exception("Exception was expected but no exception was thrown");
    }

    public IActions ActThrowsException<TException>(Func<Task> action)
        where TException : Exception
    {
        try
        {
            action.Invoke().GetAwaiter().GetResult();
        }
        catch (TException ex)
        {
            Result = ex;
            return this;
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception of type {typeof(TException).FullName} was expected but {ex.GetType().FullName} was thrown instead.", ex);
        }
        throw new Exception("Exception was expected but no exception was thrown");
    }

    public IActions Assert<T>(Action<T?> action)
    {
        action((T?)Result);
        return this;
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

    public IActions<TInstance> Act(Action<IArrangement<TInstance>> action)
    {
        action?.Invoke(Arrangement);
        return this;
    }

    public IActions<TInstance> Act(Func<IArrangement<TInstance>, object?> action)
    {
        Arrangement.Result = action?.Invoke(Arrangement);
        return this;
    }

    public IActions<TInstance> Act(Func<IArrangement<TInstance>, Task> action)
    {
        action?.Invoke(Arrangement).GetAwaiter().GetResult();
        return this;
    }


    public IActions<TInstance> Act(Func<IArrangement<TInstance>, Task<object?>> action)
    {
        Arrangement.Result = action?.Invoke(Arrangement).GetAwaiter().GetResult();
        return this;
    }

    public IActions<TInstance> ActThrowsException(Action<IArrangement<TInstance>> action)
    {
        try
        {
            action.Invoke(Arrangement);
        }
        catch (Exception ex)
        {
            Arrangement.Result = ex;
            return this;
        }
        throw new Exception("Exception was expected but no exception was thrown");
    }

    public IActions<TInstance> ActThrowsException(Func<IArrangement<TInstance>, Task> action)
    {
        try
        {
            action.Invoke(Arrangement).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Arrangement.Result = ex;
            return this;
        }
        throw new Exception("Exception was expected but no exception was thrown");
    }

    public IActions<TInstance> ActThrowsException<TException>(Action<IArrangement<TInstance>> action)
        where TException : Exception
    {
        try
        {
            action.Invoke(Arrangement);
        }
        catch (TException ex)
        {
            Arrangement.Result = ex;
            return this;
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception of type {typeof(TException).FullName} was expected but {ex.GetType().FullName} was thrown instead.", ex);
        }
        throw new Exception("Exception was expected but no exception was thrown");
    }

    public IActions<TInstance> ActThrowsException<TException>(Func<IArrangement<TInstance>, Task> action)
        where TException : Exception
    {
        try
        {
            action.Invoke(Arrangement).GetAwaiter().GetResult();
        }
        catch (TException ex)
        {
            Arrangement.Result = ex;
            return this;
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception of type {typeof(TException).FullName} was expected but {ex.GetType().FullName} was thrown instead.", ex);
        }
        throw new Exception("Exception was expected but no exception was thrown");
    }

    public IActions<TInstance> Assert(Action<IArrangement<TInstance>> action)
    {
        action?.Invoke(Arrangement);
        return this;
    }

    public IActions<TInstance> Assert(Func<IArrangement<TInstance>, Task> action)
    {
        action?.Invoke(Arrangement).GetAwaiter().GetResult();
        return this;
    }

    private Arrangement<TInstance> Arrangement { get; }
    private TInstance Service { get; }
    private IServiceProvider ServiceProvider { get; }
}
