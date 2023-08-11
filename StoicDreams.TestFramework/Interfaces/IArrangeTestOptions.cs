using Moq;

namespace StoicDreams;

public interface IArrangeTestOptions
{
    /// <summary>
    /// Add a Moq mocked service.
    /// Note: This framework will eventually deprecated Moq features due to security concerns. It is highly recommended to use one of the AddSub or AddService options.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="setupHandler"></param>
    /// <returns></returns>
    IArrangeTestOptions AddMock<TService>(Action<Mock<TService>>? setupHandler = null)
        where TService : class;

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
}
