namespace StoicDreams;

public interface IArrangeUnitOptions
{
    /// <summary>
    /// Get a service that was injected into the tested class.
    /// This might be from a Mocked, Substituted, or injected service.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="setupHandler"></param>
    /// <returns></returns>
    T GetService<T>(Action<T>? setupHandler = null) where T : class;

    /// <summary>
    /// Add messages to watch for in Console logs and throw error if any messages are found.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="messages"></param>
    /// <returns></returns>
    IArrangeUnitOptions WatchConsole(params string[] messages);
}
