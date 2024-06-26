﻿namespace StoicDreams;

public class RenderArrangement<TComponent> : IRenderArrangement<TComponent>
    where TComponent : IComponent
{
    public RenderArrangement(TestContext context, IRenderedComponent<TComponent> render)
    {
        Context = context;
        Render = render;
    }

    public IRenderedComponent<TComponent> Render { get; }

    public void DetachRender()
    {
        Context.DisposeComponents();
    }

    public T GetService<T>()
        where T : class
    {
        return Context.Services.GetService<T>() ?? throw new NullReferenceException($"Failed to get service {typeof(T).FullName}.");
    }

    public T GetResult<T>()
    {
        if (Result == null)
        {
            throw new NullReferenceException($"Expecting result of type {typeof(T).FullName} but null was returned.");
        }
        return (T)Result;
    }

    public T? GetNullableResult<T>() => (T?)Result;

    public string AlertsCssSelector { get; set; } = ".mud-alert-message";

    public FakeNavigationManager NavManager => Context.Services.GetRequiredService<FakeNavigationManager>();

    internal object? Result { get; set; }

    private TestContext Context { get; }
}
