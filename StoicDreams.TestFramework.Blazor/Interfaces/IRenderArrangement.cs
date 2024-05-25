namespace StoicDreams;

public interface IRenderArrangement<TComponent>
    where TComponent : IComponent
{
    /// <summary>
    /// Rendered component being tested
    /// </summary>
    IRenderedComponent<TComponent> Render { get; }

    /// <summary>
    /// Dispose rendered components.
    /// Simulate the rendered component being detached from the UI
    /// This calls Dispose() and/or DisposeAsync on the rendered component as well as any children that are IDisposable.
    /// </summary>
    void DetachRender();

    /// <summary>
    /// Get a service T.
    /// Throws NullReferenceException if service is missing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetService<T>() where T : class;

    /// <summary>
    /// Get the expected non-nullable result as returned by the last Act() call.
    /// Will throw exception if result value was null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetResult<T>();

    /// <summary>
    /// Get the nullable result as returned by the last Act() call.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T? GetNullableResult<T>();

    /// <summary>
    /// Simulate functionality of NavigationManager for testing purposes.
    /// </summary>
    FakeNavigationManager NavManager { get; }

    /// <summary>
    /// Set the default CSS selector to use for parsing alerts from Markup on test failures.
    /// </summary>
    string AlertsCssSelector { get; set; }
}
