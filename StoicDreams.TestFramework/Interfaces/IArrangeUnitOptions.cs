using Moq;

namespace StoicDreams;

public interface IArrangeUnitOptions
{
    /// <summary>
    /// Get a mocked service that was injected into the tested class.
    /// This will fail if a Mock was not setup for injection into class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="setupHandler"></param>
    /// <returns></returns>
    Mock<T> GetMock<T>(Action<Mock<T>>? setupHandler = null) where T : class;

    /// <summary>
    /// Get a service that was injected into the tested class.
    /// This might be from a Mocked, Substituted, or injected service.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="setupHandler"></param>
    /// <returns></returns>
    T GetService<T>(Action<T>? setupHandler = null) where T : class;
}
