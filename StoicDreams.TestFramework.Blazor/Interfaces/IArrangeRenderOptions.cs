namespace StoicDreams;

public interface IArrangeRenderOptions
{
	Dictionary<string, object> Parameters { get; }

	IArrangeRenderOptions SetupServices(Action<IServiceCollection> setupHandler);

	IArrangeRenderOptions ReplaceServiceWithMock<TService>(Action<Mock<TService>>? setupHandler = null) where TService : class;

	IArrangeRenderOptions AddStub<TComponent>() where TComponent : IComponent;

	IArrangeRenderOptions AddStub<TComponent>(string replacementMarkup) where TComponent : IComponent;

	IArrangeRenderOptions AddStub<TComponent>(RenderFragment replacementFragment) where TComponent : IComponent;

	IArrangeRenderOptions AddStub<TComponent>(Func<CapturedParameterView<TComponent>, string> replacementTemplate) where TComponent : IComponent;

	IArrangeRenderOptions AddStub<TComponent>(RenderFragment<CapturedParameterView<TComponent>> replacementTemplate) where TComponent : IComponent;

	IArrangeRenderOptions AddStub(Predicate<Type> componentTypePredicate);

	IArrangeRenderOptions AddStub(Predicate<Type> componentTypePredicate, string replacementMarkup);

	IArrangeRenderOptions AddStub(Predicate<Type> componentTypePredicate, RenderFragment replacementFragment);
}
