namespace StoicDreams;

public abstract partial class TestFramework
{
    static protected T Sub<T>(Action<T>? setupHandler = null)
        where T : class
    {
        T mock = Substitute.For<T>();
        setupHandler?.Invoke(mock);
        return mock;
    }
}