namespace StoicDreams;

/// <summary>
/// Interface containing Act and Assert scoping methods to help clearly organize tests into Arrange, Act, Assert workflows.
/// </summary>
/// <typeparam name="TInstance"></typeparam>
public interface IActions<TInstance> where TInstance : class
{
	void Act(Action<IArrangement<TInstance>> action);
	void Act(Func<IArrangement<TInstance>, object?> action);
	void Act(Func<IArrangement<TInstance>, Task> action);
	void Act(Func<IArrangement<TInstance>, Task<object?>> action);
	void Assert(Action<IArrangement<TInstance>> action);
	void Assert(Func<IArrangement<TInstance>, Task> action);
}
