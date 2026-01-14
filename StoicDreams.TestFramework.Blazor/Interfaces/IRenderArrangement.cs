using AngleSharp.Dom;

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
    /// Returns the first element from the rendered fragment Render,
    /// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal
    /// of the rendered nodes.
    /// </summary>
    /// <param name="cssSelector">The group of selectors to use.</param>
    IElement Find(string cssSelector);

    /// <summary>
    /// Returns the first element of type <typeparamref name="TElement"/> from the rendered fragment Render,
    /// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal
    /// of the rendered nodes.
    /// </summary>
    /// <typeparam name="TElement">The type of element to find (e.g., IHtmlInputElement).</typeparam>
    /// <param name="cssSelector">The group of selectors to use.</param>
    /// <exception cref="ElementNotFoundException">Thrown if no element matches the <paramref name="cssSelector"/>.</exception>
    TElement Find<TElement>(string cssSelector)
        where TElement : class, IElement;

    /// <summary>
    /// Returns a refreshable collection of <see cref="IElement"/>s from the rendered fragment Render,
    /// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal
    /// of the rendered nodes.
    /// </summary>
    /// <param name="cssSelector">The group of selectors to use.</param>
    /// <returns>An <see cref="IReadOnlyList{IElement}"/>, that can be refreshed to execute the search again.</returns>
    IReadOnlyList<IElement> FindAll(string cssSelector);

    /// <summary>
    /// Returns a collection of elements of type <typeparamref name="TElement"/> from the rendered fragment Render,
    /// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal
    /// of the rendered nodes. Only elements matching the type <typeparamref name="TElement"/> are returned.
    /// </summary>
    /// <typeparam name="TElement">The type of elements to find (e.g., IHtmlInputElement).</typeparam>
    /// <param name="cssSelector">The group of selectors to use.</param>
    /// <returns>An <see cref="IReadOnlyList{TElement}"/> containing only elements matching the specified type.</returns>
    IReadOnlyList<TElement> FindAll<TElement>(string cssSelector)
        where TElement : class, IElement;

    /// <summary>
    /// Finds the first component of type <typeparamref name="TChildComponent"/> in the render tree of
    /// Render <see cref="IRenderedComponent{TComponent}"/>.
    /// </summary>
    /// <typeparam name="TChildComponent">Type of component to find.</typeparam>
    /// <exception cref="ComponentNotFoundException">Thrown if a component of type <typeparamref name="TChildComponent"/> was not found in the render tree.</exception>
    /// <returns>The <see cref="RenderedComponent{T}"/>.</returns>
    IRenderedComponent<TChildComponent> FindComponent<TChildComponent>()
        where TChildComponent : IComponent;

    /// <summary>
	/// Finds all components of type <typeparamref name="TChildComponent"/> in the render tree of
	/// Render <see cref="IRenderedComponent{TComponent}"/>, in depth-first order.
	/// </summary>
	/// <typeparam name="TChildComponent">Type of components to find.</typeparam>
	/// <returns>The <see cref="RenderedComponent{T}"/>s.</returns>
    IReadOnlyList<IRenderedComponent<TChildComponent>> FindComponents<TChildComponent>()
        where TChildComponent : IComponent;

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
    NavigationManager NavManager { get; }

    /// <summary>
    /// Set the default CSS selector to use for parsing alerts from Markup on test failures.
    /// </summary>
    string AlertsCssSelector { get; set; }
}
