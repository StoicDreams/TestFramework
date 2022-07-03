namespace StoicDreams;

public abstract partial class TestFramework
{
	static protected Mock<T> Mock<T>(Action<Mock<T>>? setupHandler = null)
		where T : class
	{
		Mock<T> mock = new();
		setupHandler?.Invoke(mock);
		return mock;
	}

	static protected T IsAny<T>() => It.IsAny<T>();
}