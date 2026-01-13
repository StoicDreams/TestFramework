namespace StoicDreams;

public abstract class TestFrameworkBlazor : TestFramework
{
    protected static RenderFragment MockRender(string content)
    {
        return builder =>
        {
            builder.AddMarkupContent(0, content);
        };
    }

    protected static IRenderActions<TComponent> ArrangeRenderTest<TComponent>(
        Action<IArrangeRenderOptions>? arrangeHandler = null,
        params Func<IServiceCollection, IServiceCollection>[] startupHandlers
        )
        where TComponent : IComponent
    {
        BunitContext context = new();
        context.JSInterop.Mode = JSRuntimeMode.Loose;
        ArrangeRenderOptions options = new(context);
        arrangeHandler?.Invoke(options);
        foreach (Func<IServiceCollection, IServiceCollection> handler in startupHandlers)
        {
            handler.Invoke(context.Services);
        }
        IRenderedComponent<TComponent> render = context.Render<TComponent>(builder =>
        {
            foreach (string key in options.Parameters.Keys)
            {
                builder.TryAdd(key, options.Parameters[key]);
            }
            foreach (object handler in options.ParamHandlers)
            {
                if (handler is Action<ComponentParameterCollectionBuilder<TComponent>> tHandler)
                {
                    tHandler.Invoke(builder);
                }
            }
        });
        RenderActions<TComponent> actions = new(context, render);
        actions.SetConsoleWatch(options.ConsoleWatch);
        return actions;
    }
}
