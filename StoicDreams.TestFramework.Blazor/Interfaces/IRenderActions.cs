namespace StoicDreams;

public interface IRenderActions<TComponent>
	where TComponent : IComponent
{
	/// <summary>
	/// Default Act processing.
	/// Followup Assert can call GetResult<string>() to get rendered markup from this Act process.
	/// </summary>
	/// <param name="action"></param>
	IRenderActions<TComponent> Act(Action<IRenderArrangement<TComponent>>? action = null);

	/// <summary>
	/// Act processing with explicit result returned.
	/// Follow-up Assert can call GetResult<T>() to get result value.
	/// </summary>
	/// <param name="action"></param>
	IRenderActions<TComponent> Act(Func<IRenderArrangement<TComponent>, object?> action);

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
