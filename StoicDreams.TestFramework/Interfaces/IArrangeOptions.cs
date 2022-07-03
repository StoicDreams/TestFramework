namespace StoicDreams;

public interface IArrangeOptions
{
	Mock<T> GetMock<T>(Action<Mock<T>>? setupHandler = null) where T: class;
}
