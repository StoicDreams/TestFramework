namespace StoicDreams;

public abstract partial class TestFramework
{
    static protected T Mock<T>(Action<T>? setupHandler = null)
        where T : class
    {
        T mock = Substitute.For<T>();
        setupHandler?.Invoke(mock);
        return mock;
    }

    static protected T IsAny<T>() => Arg.Any<T>();
}