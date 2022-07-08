using Bunit;

namespace StoicDreams;

public class ArrangeRenderOptions : IArrangeRenderOptions
{
	public ArrangeRenderOptions(TestContext context)
	{
		Context = context;
	}

	public Dictionary<string, object> Parameters { get; } = new();

	public IArrangeRenderOptions ReplaceServiceWithMock<TService>(Action<Mock<TService>>? setupHandler = null)
		where TService : class
	{
		Context.Services.AddMock(setupHandler);
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
		Context.ComponentFactories.AddStub<TComponent>(replacementTemplate);
		return this;
	}

	public IArrangeRenderOptions AddStub<TComponent>(RenderFragment<CapturedParameterView<TComponent>> replacementTemplate)
		 where TComponent : IComponent
	{
		Context.ComponentFactories.AddStub<TComponent>(replacementTemplate);
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

	private TestContext Context { get; }
}
