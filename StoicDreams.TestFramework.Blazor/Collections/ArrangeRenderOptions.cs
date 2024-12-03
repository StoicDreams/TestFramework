namespace StoicDreams;

public class ArrangeRenderOptions : IArrangeRenderOptions
{
    public ArrangeRenderOptions(TestContext context)
    {
        Context = context;
    }

    public Dictionary<string, object> Parameters { get; } = [];
    public List<object> ParamHandlers { get; } = [];

    public IArrangeRenderOptions SetupComponentParameters<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>> setupHandler) where TComponent : IComponent
    {
        if (setupHandler is object handler)
        {
            ParamHandlers.Add(handler);
        }
        return this;
    }

    public IArrangeRenderOptions SetupServices(Action<IServiceCollection> setupHandler)
    {
        setupHandler.Invoke(Context.Services);
        return this;
    }

    public IArrangeRenderOptions SetupServices(Func<IServiceCollection, IServiceCollection> setupHandler)
    {
        setupHandler.Invoke(Context.Services);
        return this;
    }

    public IArrangeRenderOptions AddStub<TComponent>()
         where TComponent : IComponent
    {
        Context.ComponentFactories.AddStub<TComponent>();
        return this;
    }

    public IArrangeRenderOptions AddStub<TComponent>(string replacementMarkup)
         where TComponent : IComponent
    {
        Context.ComponentFactories.AddStub<TComponent>(replacementMarkup);
        return this;
    }

    public IArrangeRenderOptions AddStub<TComponent>(RenderFragment replacementFragment)
         where TComponent : IComponent
    {
        Context.ComponentFactories.AddStub<TComponent>(replacementFragment);
        return this;
    }

    public IArrangeRenderOptions AddStub<TComponent>(Func<CapturedParameterView<TComponent>, string> replacementTemplate)
         where TComponent : IComponent
    {
        Context.ComponentFactories.AddStub(replacementTemplate);
        return this;
    }

    public IArrangeRenderOptions AddStub<TComponent>(RenderFragment<CapturedParameterView<TComponent>> replacementTemplate)
         where TComponent : IComponent
    {
        Context.ComponentFactories.AddStub(replacementTemplate);
        return this;
    }

    public IArrangeRenderOptions AddStub(Predicate<Type> componentTypePredicate)
    {
        Context.ComponentFactories.AddStub(componentTypePredicate);
        return this;
    }

    public IArrangeRenderOptions AddStub(Predicate<Type> componentTypePredicate, string replacementMarkup)
    {
        Context.ComponentFactories.AddStub(componentTypePredicate, replacementMarkup);
        return this;
    }

    public IArrangeRenderOptions AddStub(Predicate<Type> componentTypePredicate, RenderFragment replacementFragment)
    {
        Context.ComponentFactories.AddStub(componentTypePredicate, replacementFragment);
        return this;
    }

    public IArrangeRenderOptions WatchConsole(params string[] messages)
    {
        ConsoleWatch = messages;
        return this;
    }

    public TestContext Context { get; }
    internal string[] ConsoleWatch { get; set; } = [];
}
