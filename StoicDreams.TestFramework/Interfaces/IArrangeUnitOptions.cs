namespace StoicDreams;

public interface IArrangeUnitOptions
{
	Mock<T> GetMock<T>(Action<Mock<T>>? setupHandler = null) where T: class;
}
