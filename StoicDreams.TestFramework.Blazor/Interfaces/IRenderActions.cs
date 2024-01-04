namespace StoicDreams;

public interface IRenderActions<TComponent>
    where TComponent : IComponent
{
    /// <summary>
    /// Default Act processing.
    /// Follow-up Assert can call GetResult<string>() to get rendered markup from this Act process.
    /// </summary>
    /// <param name="action"></param>
    IRenderActions<TComponent> Act(Action<IRenderArrangement<TComponent>>? action = null);

    /// <summary>
    /// Default Act processing.
    /// Follow-up Assert can call GetResult<string>() to get rendered markup from this Act process.
    /// </summary>
    /// <param name="action"></param>
    IRenderActions<TComponent> Act(Func<IRenderArrangement<TComponent>, Task>? action = null);

    /// <summary>
    /// Act processing with explicit result returned.
    /// Follow-up Assert can call GetResult<T>() to get result value.
    /// </summary>
    /// <param name="action"></param>
    IRenderActions<TComponent> Act(Func<IRenderArrangement<TComponent>, object?> action);

    /// <summary>
    /// Act processing with explicit result returned.
    /// Follow-up Assert can call GetResult<T>() to get result value.
    /// </summary>
    /// <param name="action"></param>
    IRenderActions<TComponent> Act(Func<IRenderArrangement<TComponent>, Task<object?>> action);

    /// <summary>
    /// Act processing that is expected to throw any exception.
    /// Exception message will be returned as result.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    IRenderActions<TComponent> ActThrowsException(Action<IRenderArrangement<TComponent>>? action = null);

    /// <summary>
    /// Act processing that is expected to throw any exception.
    /// Exception message will be returned as result.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    IRenderActions<TComponent> ActThrowsException(Func<IRenderArrangement<TComponent>, Task>? action = null);

    /// <summary>
    /// Act processing that is expected to throw an explicit exception type.
    /// Exception message will be returned as result.
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    IRenderActions<TComponent> ActThrowsException<TException>(Action<IRenderArrangement<TComponent>>? action = null) where TException : Exception;

    /// <summary>
    /// Act processing that is expected to throw an explicit exception type.
    /// Exception message will be returned as result.
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    IRenderActions<TComponent> ActThrowsException<TException>(Func<IRenderArrangement<TComponent>, Task>? action = null) where TException : Exception;

    /// <summary>
    /// Assertion processing.
    /// </summary>
    /// <param name="action"></param>
    IRenderActions<TComponent> Assert(Action<IRenderArrangement<TComponent>> action);

    /// <summary>
    /// Assertion processing for assertions that call asyncronous commands.
    /// </summary>
    /// <param name="action"></param>
    IRenderActions<TComponent> Assert(Func<IRenderArrangement<TComponent>, Task> action);
}
