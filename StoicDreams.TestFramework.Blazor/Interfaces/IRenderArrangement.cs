namespace StoicDreams;

public interface IRenderArrangement<TComponent>
	where TComponent : IComponent
{
	IRenderedComponent<TComponent> Render { get; }

	Mock<T> GetMock<T>() where T : class;

	T GetResult<T>();

	T? GetNullableResult<T>();
	
	FakeNavigationManager NavManager { get; }
}
