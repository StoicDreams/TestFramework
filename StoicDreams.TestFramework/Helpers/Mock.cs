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

    internal Mock(T instance)
    {
        _instance = instance;
    }

    public Mock<T> Setup<R>(Func<T, R> setup)
    {
        setup.Invoke(_instance);
        return this;
    }

    public static implicit operator Mock<T>(T instance) => new(instance);

    private T _instance { get; }
    public T Object => _instance;

    public Mock<T> Setup(Action<T> setup)
    {
        setup.Invoke(_instance);
        return this;
    }

    public Mock<T> Verify(Action<T> setup)
    {
        setup.Invoke(_instance.Received());
        return this;
    }

    public Mock<T> Verify(Action<T> setup, Times times)
    {
        if (times == 0)
        {
            setup.Invoke(_instance.DidNotReceive());
        }
        else
        {
            setup.Invoke(_instance.Received(times));
        }
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

    public Mock<T> ReturnsAsync<A>(A returnThis)
    {
        return this.Returns<A>(_ => returnThis!);
    }

    public Mock<T> ReturnsAsync<A>(Func<A, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0]));
        return this;
    }

    public Mock<T> ReturnsAsync<A, B>(Func<A, B, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0], (B)ci[1]));
        return this;
    }

    public Mock<T> ReturnsAsync<A, B, C>(Func<A, B, C, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0], (B)ci[1], (C)ci[2]));
        return this;
    }

    public Mock<T> ReturnsAsync<A, B, C, D>(Func<A, B, C, D, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0], (B)ci[1], (C)ci[2], (D)ci[3]));
        return this;
    }

    public Mock<T> ReturnsAsync<A, B, C, D, E>(Func<A, B, C, D, E, object> returnThis)
    {
        _instance.Returns(ci => returnThis((A)ci[0], (B)ci[1], (C)ci[2], (D)ci[3], (E)ci[4]));
        return this;
    }

    [Obsolete("""
        This TestFramework is no longer using Moq and this method of validation is not supported by NSubstitute.
        Each desire call must be individually and explicitly validated within assertions.
        """)]
    public void Verify()
    {
    }

    [Obsolete("""
        This TestFramework is no longer using Moq and this method of validation is not supported by NSubstitute.
        Each desire call must be individually and explicitly validated within assertions.
        """)]
    public void VerifyAll()
    {
    }
}
