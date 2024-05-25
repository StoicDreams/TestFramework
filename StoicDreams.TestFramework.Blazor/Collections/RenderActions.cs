namespace StoicDreams;

public class RenderActions<TComponent> : IRenderActions<TComponent>
    where TComponent : IComponent
{
    internal RenderActions(TestContext context, IRenderedComponent<TComponent> render)
    {
        Arrangement = new RenderArrangement<TComponent>(context, render);
    }

    public IRenderActions<TComponent> Act(Action<IRenderArrangement<TComponent>>? action = null)
    {
        Arrangement.Result = null;
        try
        {
            action?.Invoke(Arrangement);
            if (Arrangement.Render.IsDisposed)
            {
                Arrangement.Result = "The component has been removed from the render tree";
            }
            else
            {
                Arrangement.Result = Arrangement.Render.Markup;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(Arrangement.BuildExceptionMessage(ex.Message));
        }
        return this;
    }

    public IRenderActions<TComponent> Act(Func<IRenderArrangement<TComponent>, Task>? action = null)
    {
        try
        {
            Arrangement.Result = null;
            action?.Invoke(Arrangement).GetAwaiter().GetResult();
            Arrangement.Result = Arrangement.Render.Markup;
        }
        catch (Exception ex)
        {
            throw new Exception(Arrangement.BuildExceptionMessage(ex.Message));
        }
        return this;
    }

    public IRenderActions<TComponent> ActThrowsException(Action<IRenderArrangement<TComponent>>? action = null)
    {
        Arrangement.Result = null;
        try
        {
            action?.Invoke(Arrangement);
        }
        catch (Exception ex)
        {
            Arrangement.Result = ex.Message;
            return this;
        }

        throw new Exception(Arrangement.BuildExceptionMessage("Expected exception was not thrown."));
    }

    public IRenderActions<TComponent> ActThrowsException(Func<IRenderArrangement<TComponent>, Task>? action = null)
    {
        Arrangement.Result = null;
        try
        {
            action?.Invoke(Arrangement).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Arrangement.Result = ex.Message;
            return this;
        }

        throw new Exception(Arrangement.BuildExceptionMessage("Expected exception was not thrown."));
    }

    public IRenderActions<TComponent> ActThrowsException<TException>(Action<IRenderArrangement<TComponent>>? action = null)
        where TException : Exception
    {
        Arrangement.Result = null;
        action?.Invoke(Arrangement);
        try
        {
            Arrangement.Result = Arrangement.Render.Markup;
        }
        catch (TException expected)
        {
            Arrangement.Result = expected.Message;
            return this;
        }
        catch (Exception) { }
        throw new Exception(Arrangement.BuildExceptionMessage($"Expected exception of type {typeof(TException).Name} was not thrown."));
    }

    public IRenderActions<TComponent> ActThrowsException<TException>(Func<IRenderArrangement<TComponent>, Task>? action = null)
        where TException : Exception
    {
        Arrangement.Result = null;
        try
        {
            action?.Invoke(Arrangement).GetAwaiter().GetResult();
        }
        catch (TException expected)
        {
            Arrangement.Result = expected.Message;
            return this;
        }
        catch (Exception) { }
        throw new Exception(Arrangement.BuildExceptionMessage($"Expected exception of type {typeof(TException).Name} was not thrown."));
    }

    public IRenderActions<TComponent> Act(Func<IRenderArrangement<TComponent>, object?> action)
    {
        try
        {
            Arrangement.Result = action?.Invoke(Arrangement);
        }
        catch (Exception ex)
        {
            throw new Exception(Arrangement.BuildExceptionMessage(ex.Message));
        }
        return this;
    }

    public IRenderActions<TComponent> Act(Func<IRenderArrangement<TComponent>, Task<object?>> action)
    {
        try
        {
            Arrangement.Result = action?.Invoke(Arrangement).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            throw new Exception(Arrangement.BuildExceptionMessage(ex.Message));
        }
        return this;
    }

    public IRenderActions<TComponent> Assert(Action<IRenderArrangement<TComponent>> action)
    {
        try
        {
            action.Invoke(Arrangement);
        }
        catch (Exception ex)
        {
            throw new Exception(Arrangement.BuildExceptionMessage(ex.Message));
        }
        return this;
    }

    public IRenderActions<TComponent> Assert(Func<IRenderArrangement<TComponent>, Task> action)
    {
        try
        {
            Task.WaitAny(action.Invoke(Arrangement));
        }
        catch (Exception ex)
        {
            throw new Exception(Arrangement.BuildExceptionMessage(ex.Message));
        }
        return this;
    }

    private RenderArrangement<TComponent> Arrangement { get; }
}
