using NSubstitute.Core;

namespace StoicDreams;

/// <summary>
/// Special extensions to simulate Moq Mock methods to help transition from Moq to NSubstitute.
/// </summary>
public static class MockExtensions
{
    public static T Setup<T>(this T self, Func<T, T> setup)
        where T : class
    {
        setup.Invoke(self);
        return self;
    }


    public static ConfiguredCall ReturnsAsync<T>(this T self, T returnThis)
        where T : class
    {
        return self.Returns(returnThis);
    }

    public static ConfiguredCall ReturnsAsync<T>(this T self, Func<T> returnThis)
        where T : class
    {
        return self.Returns(ci => returnThis());
    }

    public static ConfiguredCall ReturnsAsync<T, A>(this T self, Func<A, T> returnThis)
        where T : class
    {
        return self.Returns(ci => returnThis((A)ci[0]));
    }

    public static ConfiguredCall ReturnsAsync<T, A, B>(this T self, Func<A, B, T> returnThis)
        where T : class
    {
        return self.Returns(ci => returnThis((A)ci[0], (B)ci[1]));
    }

    public static ConfiguredCall ReturnsAsync<T, A, B, C>(this T self, Func<A, B, C, T> returnThis)
        where T : class
    {
        return self.Returns(ci => returnThis((A)ci[0], (B)ci[1], (C)ci[2]));
    }

    public static ConfiguredCall ReturnsAsync<T, A, B, C, D>(this T self, Func<A, B, C, D, T> returnThis)
        where T : class
    {
        return self.Returns(ci => returnThis((A)ci[0], (B)ci[1], (C)ci[2], (D)ci[3]));
    }
}
