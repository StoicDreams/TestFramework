using System.Linq.Expressions;

namespace StoicDreams;

public abstract partial class TestFramework
{
    /// <summary>
    /// Create a substitute for an interface or class with virtual members.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="setupHandler"></param>
    /// <returns></returns>
    static protected T Sub<T>(Action<T>? setupHandler = null)
        where T : class
    {
        T mock = Substitute.For<T>();
        setupHandler?.Invoke(mock);
        return mock;
    }

    /// <summary>
    /// Use in method arrangement to accept any of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    static protected T IsAny<T>() => Arg.Any<T>();

    /// <summary>
    /// Use in method arrangement to accept a specific value of type T only.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    static protected T Is<T>(T value) => Arg.Is(value);

    /// <summary>
    /// Use in method arrangement to evaluate given expression to determine if T input is valid for arrangement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="predicate"></param>
    /// <returns></returns>
    static protected T Is<T>(Expression<Predicate<T>> predicate) => Arg.Is(predicate);

    /// <summary>
    /// Use in method arrangement to evaluate given expression to determine if object input is valid for arrangement.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    static protected object Is(Expression<Predicate<object>> predicate) => Arg.Is(predicate);
}