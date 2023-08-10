namespace StoicDreams;

public interface IArrangement<TInstance> where TInstance : class
{
    /// <summary>
    /// Get the service being tested.
    /// </summary>
    TInstance Service { get; }

    /// <summary>
    /// Get an expected result returned by IActions.Act command.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetResult<T>();

    /// <summary>
    /// Get an expected result that may include null values returned by IActions.Act command.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T? GetNullableResult<T>();

    /// <summary>
    /// Get a mock created during test arrangment.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetMock<T>() where T : class;

    /// <summary>
    /// Get a service created during test arrangment.
    /// Will throw NotImplementedException if service is missing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetService<T>();
}
