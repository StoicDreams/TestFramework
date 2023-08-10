namespace StoicDreams;

public interface IArrangeIntegrationOptions
{
    IArrangeIntegrationOptions ReplaceServiceWithMock<TService>(Action<TService>? setupHandler = null) where TService : class;
    IServiceCollection Services { get; }
}
