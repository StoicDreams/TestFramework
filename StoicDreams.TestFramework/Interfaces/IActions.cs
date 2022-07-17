namespace StoicDreams;

public interface IActions
{
	/// <summary>
	/// Act on test, returning result of action.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="action"></param>
	void Act<T>(Func<T, object> action);

	/// <summary>
	/// Act on test, expecting an error to be thrown.
	/// </summary>
	/// <param name="action"></param>
	void ActThrowsException(Action action);

	/// <summary>
	/// Assert results of test, being passed result from last Act result.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="action"></param>
	void Assert<T>(Action<T?> action);
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
	void Act(Action<IArrangement<TInstance>> action);

	/// <summary>
	/// Act on test and return a result.
	/// </summary>
	/// <param name="action"></param>
	void Act(Func<IArrangement<TInstance>, object?> action);

	/// <summary>
	/// Asyncronous Act on test.
	/// </summary>
	/// <param name="action"></param>
	void ActAsync(Func<IArrangement<TInstance>, Task> action);

	/// <summary>
	/// Asyncronous act on test, returning result.
	/// </summary>
	/// <param name="action"></param>
	void Act(Func<IArrangement<TInstance>, Task<object?>> action);

	/// <summary>
	/// Run assertions on results of test.
	/// </summary>
	/// <param name="action"></param>
	void Assert(Action<IArrangement<TInstance>> action);

	/// <summary>
	/// Asyncronous run of assertions on results of test.
	/// </summary>
	/// <param name="action"></param>
	void AssertAsync(Func<IArrangement<TInstance>, Task> action);
}
