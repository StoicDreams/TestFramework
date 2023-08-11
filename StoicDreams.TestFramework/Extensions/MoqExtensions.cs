namespace StoicDreams;

public static class MoqExtensions
{
    public static Mock<T> Setup<T, R>(this T instance, Func<T, R> setup)
        where T : class
    {
        Mock<T> mock = new(instance);
        return mock.Setup(setup);
    }
}
