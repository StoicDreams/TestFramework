namespace StoicDreams;

/// <summary>
/// Helper for transitioning legacy tests using Moq to NSubstitute.
/// </summary>
public class Mock<T>
    where T : class
{
    public Mock()
    {
        _instance = Substitute.For<T>();
    }

    private Mock(T instance)
    {
        _instance = instance;
    }

    public static implicit operator Mock<T>(T instance) => new(instance);

    private T _instance { get; }
    public T Object => _instance;

    public Mock<T> Setup(Action<T> setup)
    {
        setup.Invoke(_instance);
        return this;
    }

    public Mock<T> Callback<A>(Func<A, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0]));
        return this;
    }

    public Mock<T> Callback<A, B>(Func<A, B, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0], (B)ci[1]));
        return this;
    }

    public Mock<T> Callback<A, B, C>(Func<A, B, C, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0], (B)ci[1], (C)ci[2]));
        return this;
    }

    public Mock<T> Callback<A, B, C, D>(Func<A, B, C, D, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0], (B)ci[1], (C)ci[2], (D)ci[3]));
        return this;
    }

    public Mock<T> Callback<A, B, C, D, E>(Func<A, B, C, D, E, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0], (B)ci[1], (C)ci[2], (D)ci[3], (E)ci[4]));
        return this;
    }

    public Mock<T> Returns<A>(A returnThis)
    {
        return this.Returns<A>(_ => returnThis!);
    }

    public Mock<T> Returns<A>(Func<A, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0]));
        return this;
    }

    public Mock<T> Returns<A, B>(Func<A, B, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0], (B)ci[1]));
        return this;
    }

    public Mock<T> Returns<A, B, C>(Func<A, B, C, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0], (B)ci[1], (C)ci[2]));
        return this;
    }

    public Mock<T> Returns<A, B, C, D>(Func<A, B, C, D, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0], (B)ci[1], (C)ci[2], (D)ci[3]));
        return this;
    }

    public Mock<T> Returns<A, B, C, D, E>(Func<A, B, C, D, E, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0], (B)ci[1], (C)ci[2], (D)ci[3], (E)ci[4]));
        return this;
    }
}
