namespace StoicDreams;

public interface IArrangeTestOptions
{
    /// <summary>
    /// Add a substituted service.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="setupHandler"></param>
    /// <returns></returns>
    IArrangeTestOptions AddSub<TService>(Action<TService>? setupHandler = null)
        where TService : class;

    /// <summary>
    /// Add a service from a given class.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    IArrangeTestOptions AddService<TService>()
        where TService : class;

    /// <summary>
    /// Add a service from a given interface and class.
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    IArrangeTestOptions AddService<TInterface, TService>()
        where TInterface : class
        where TService : class, TInterface;

    /// <summary>
    /// Add a service that you explicitly create and setup.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="setupHandler"></param>
    /// <returns></returns>
    IArrangeTestOptions AddService<TService>(Func<TService> setupHandler)
        where TService : class;

    /// <summary>
    /// Add a service created from NSubstitute's Substitute.For method.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="setupHandler"></param>
    /// <returns></returns>
    IArrangeTestOptions AddService<TService>(Func<TService, TService> setupHandler)
        where TService : class;

    /// <summary>
    /// Add messages to watch for in Console logs and throw error if any messages are found.
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    IArrangeTestOptions WatchConsole(params string[] messages);
}
