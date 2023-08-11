using Moq;

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

    static protected Mock<T> Mock<T>(Action<Mock<T>>? setupHandler = null)
        where T : class
    {
        Mock<T> mock = new();
        setupHandler?.Invoke(mock);
        return mock;
    }

    static protected T IsAny<T>() => It.IsAny<T>();
}