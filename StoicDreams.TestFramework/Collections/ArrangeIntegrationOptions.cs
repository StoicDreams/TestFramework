using Microsoft.Extensions.DependencyInjection.Extensions;

namespace StoicDreams;

public class ArrangeIntegrationOptions : IArrangeIntegrationOptions
{
	public ArrangeIntegrationOptions(IServiceCollection services)
	{
		Services = services;
	}

	public IArrangeIntegrationOptions ReplaceServiceWithMock<TService>(Action<Mock<TService>>? setupHandler = null)
		where TService : class
	{
		Mock<TService> mock = new();
		setupHandler?.Invoke(mock);
		Services.Replace(new ServiceDescriptor(typeof(TService), mock.Object));
		return this;
	}

	public IServiceCollection Services { get; }
}
