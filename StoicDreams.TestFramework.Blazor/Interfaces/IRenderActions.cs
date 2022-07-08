namespace StoicDreams;

public interface IRenderActions<TComponent>
	where TComponent : IComponent
{
	void Act(Action<IRenderArrangement<TComponent>> action);

	void Act(Func<IRenderArrangement<TComponent>, object?> action);

	void Assert(Action<IRenderArrangement<TComponent>> action);

	void Assert(Func<IRenderArrangement<TComponent>, Task> action);
}
