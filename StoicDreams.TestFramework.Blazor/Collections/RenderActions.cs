namespace StoicDreams;

public class RenderActions<TComponent> : IRenderActions<TComponent>
	where TComponent : IComponent
{
	internal RenderActions(TestContext context, IRenderedComponent<TComponent> render)
	{
		Arrangement = new RenderArrangement<TComponent>(context, render);
	}

	public void Act(Action<IRenderArrangement<TComponent>>? action = null)
	{
		Arrangement.Result = null;
		action?.Invoke(Arrangement);
		try
		{
			Arrangement.Result = Arrangement.Render.Markup;
		}
		catch (Exception ex)
		{
			Arrangement.Result = ex.Message;
		}
	}

	public void Act(Func<IRenderArrangement<TComponent>, object?> action)
	{
		Arrangement.Result = action?.Invoke(Arrangement);
	}

	public void Assert(Action<IRenderArrangement<TComponent>> action)
	{
		action.Invoke(Arrangement);
	}

	public void Assert(Func<IRenderArrangement<TComponent>, Task> action)
	{
		Task.WaitAll(action.Invoke(Arrangement));
	}

	private RenderArrangement<TComponent> Arrangement { get; }
}
