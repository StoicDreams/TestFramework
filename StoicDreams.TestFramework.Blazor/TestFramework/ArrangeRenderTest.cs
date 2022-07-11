namespace StoicDreams;

public abstract class TestFrameworkBlazor : TestFramework
{
	protected RenderFragment MockRender(string content)
	{
		return builder =>
		{
			builder.AddMarkupContent(0, content);
		};
	}

	protected IRenderActions<TComponent> ArrangeRenderTest<TComponent>(
		Action<IArrangeRenderOptions>? arrangeHandler = null,
		params Func<IServiceCollection, IServiceCollection>[] startupHandlers
		)
		where TComponent : IComponent
	{
		TestContext context = new();
		context.JSInterop.Mode = JSRuntimeMode.Loose;
		ArrangeRenderOptions options = new(context);
		arrangeHandler?.Invoke(options);
		foreach (Func<IServiceCollection, IServiceCollection> handler in startupHandlers)
		{
			handler.Invoke(context.Services);
		}
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
