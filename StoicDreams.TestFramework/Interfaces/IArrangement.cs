namespace StoicDreams;

public interface IArrangement<TInstance> where TInstance : class
{
	/// <summary>
	/// Get the service being tested.
	/// </summary>
	TInstance Service { get; }

	/// <summary>
	/// Get result returned by IActions.Act command.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	T? GetResult<T>();

	/// <summary>
	/// Get a mock created during test arrangment.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	Mock<T> GetMock<T>() where T : class;
}
