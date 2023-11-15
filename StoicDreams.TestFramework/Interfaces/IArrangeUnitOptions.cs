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
}
