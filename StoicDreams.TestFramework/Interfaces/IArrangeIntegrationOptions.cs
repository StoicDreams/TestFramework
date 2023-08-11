using Moq;

namespace StoicDreams;

public interface IArrangeIntegrationOptions
{
    IArrangeIntegrationOptions ReplaceServiceWithMock<TService>(Action<Mock<TService>>? setupHandler = null) where TService : class;
    IArrangeIntegrationOptions ReplaceServiceWithSub<TService>(Action<TService>? setupHandler = null) where TService : class;
    IServiceCollection Services { get; }
}
