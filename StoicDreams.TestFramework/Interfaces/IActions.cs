namespace StoicDreams;

/// <summary>
/// Interface containing Act and Assert scoping methods to help clearly organize tests into Arrange, Act, Assert workflows.
/// </summary>
public interface IActions
{
    /// <summary>
    /// Act on test, returning result of action.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    IActions Act<T>(Func<T, object> action);

    /// <summary>
    /// Act on test, returning result of action from async method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    IActions Act<T>(Func<T, Task<object>> action);

    /// <summary>
    /// Act on test, expecting an error to be thrown.
    /// </summary>
    /// <param name="action"></param>
    IActions ActThrowsException(Action action);

    /// <summary>
    /// Act on test, expecting a specific error to be thrown.
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="action"></param>
    IActions ActThrowsException<TException>(Action action) where TException : Exception;

    /// <summary>
    /// Act on test, expecting an error to be thrown from async method.
    /// </summary>
    /// <param name="action"></param>
    IActions ActThrowsException(Func<Task> action);

    /// <summary>
    /// Act on test, expecting a specific error to be thrown from async method.
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="action"></param>
    IActions ActThrowsException<TException>(Func<Task> action) where TException : Exception;

    /// <summary>
    /// Assert results of test, being passed result from last Act result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    IActions Assert<T>(Action<T?> action);
}

/// <summary>
/// Interface containing Act and Assert scoping methods to help clearly organize tests into Arrange, Act, Assert workflows.
/// </summary>
/// <typeparam name="TInstance"></typeparam>
public interface IActions<TInstance> where TInstance : class
{
    /// <summary>
    /// Act on test with no result.
    /// </summary>
    /// <param name="action"></param>
    IActions<TInstance> Act(Action<IArrangement<TInstance>> action);

    /// <summary>
    /// Act on test and return a result.
    /// </summary>
    /// <param name="action"></param>
    IActions<TInstance> Act(Func<IArrangement<TInstance>, object?> action);

    /// <summary>
    /// Act on test.
    /// </summary>
    /// <param name="action"></param>
    IActions<TInstance> Act(Func<IArrangement<TInstance>, Task> action);

    /// <summary>
    /// Act on test, returning result from async method.
    /// </summary>
    /// <param name="action"></param>
    IActions<TInstance> Act(Func<IArrangement<TInstance>, Task<object?>> action);

    /// <summary>
    /// Act on test, expecting an error to be thrown calling method.
    /// </summary>
    /// <param name="action"></param>
    IActions<TInstance> ActThrowsException(Action<IArrangement<TInstance>> action);

    /// <summary>
    /// Act on test, expecting an error to be thrown calling async method.
    /// </summary>
    /// <param name="action"></param>
    IActions<TInstance> ActThrowsException(Func<IArrangement<TInstance>, Task> action);

    /// <summary>
    /// Act on test, expecting an error to be thrown calling method.
    /// </summary>
    /// <param name="action"></param>
    IActions<TInstance> ActThrowsException<TException>(Action<IArrangement<TInstance>> action) where TException : Exception;

    /// <summary>
    /// Act on test, expecting an error to be thrown calling async method.
    /// </summary>
    /// <param name="action"></param>
    IActions<TInstance> ActThrowsException<TException>(Func<IArrangement<TInstance>, Task> action) where TException : Exception;

    /// <summary>
    /// Run assertions on results of test.
    /// </summary>
    /// <param name="action"></param>
    IActions<TInstance> Assert(Action<IArrangement<TInstance>> action);

    /// <summary>
    /// Run assertions on results of test with ability to call async methods.
    /// </summary>
    /// <param name="action"></param>
    IActions<TInstance> Assert(Func<IArrangement<TInstance>, Task> action);
}
