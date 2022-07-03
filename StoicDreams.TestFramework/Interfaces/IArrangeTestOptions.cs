namespace StoicDreams;

public interface IArrangeTestOptions
{
	IArrangeTestOptions AddMock<TService>(Action<Mock<TService>>? setupHandler = null)
		where TService : class;

	IArrangeTestOptions AddService<TService>()
		where TService : class;

	IArrangeTestOptions AddService<TInterface, TService>()
		where TInterface : class
		where TService : class, TInterface;

	IArrangeTestOptions AddService<TService>(Func<TService> setupHandler)
		where TService : class;
}
