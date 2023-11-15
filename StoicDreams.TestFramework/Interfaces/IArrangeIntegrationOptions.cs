namespace StoicDreams;

public interface IArrangeIntegrationOptions
{
    IArrangeIntegrationOptions ReplaceServiceWithSub<TService>(Action<TService>? setupHandler = null) where TService : class;
    IServiceCollection Services { get; }
}
