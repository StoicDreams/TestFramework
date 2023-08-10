namespace StoicDreams;

public interface IArrangeUnitOptions
{
    T GetMock<T>(Action<T>? setupHandler = null) where T : class;
}
