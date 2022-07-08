namespace StoicDreams;

public abstract class TestFrameworkBlazor : TestFramework
{
	public IRenderActions<TComponent> ArrangeRenderTest<TComponent>(
		Action<IArrangeRenderOptions>? arrangeHandler = null,
		params Func<IServiceCollection, IServiceCollection>[] startupHandlers
		)
		where TComponent : IComponent
	{
		TestContext context = new();
		ArrangeRenderOptions options = new(context);
		arrangeHandler?.Invoke(options);
		IRenderedComponent<TComponent> render = context.RenderComponent<TComponent>(builder =>
		{
			foreach(string key in options.Parameters.Keys)
			{
				builder.TryAdd(key, options.Parameters[key]);
			}
		});
		RenderActions<TComponent> actions = new(context, render);
		return actions;
	}
}
