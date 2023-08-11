using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace StoicDreams;

public class ArrangeIntegrationOptions : IArrangeIntegrationOptions
{
    public ArrangeIntegrationOptions(IServiceCollection services)
    {
        Services = services;
    }

    public IArrangeIntegrationOptions ReplaceServiceWithSub<TService>(Action<TService>? setupHandler = null)
        where TService : class
    {
        TService mock = Substitute.For<TService>();
        setupHandler?.Invoke(mock);
        Services.Replace(new ServiceDescriptor(typeof(TService), mock));
        return this;
    }

    public IArrangeIntegrationOptions ReplaceServiceWithMock<TService>(Action<Mock<TService>>? setupHandler = null)
        where TService : class
    {
        Mock<TService> mock = new();
        Services.AddSingleton(mock);
        setupHandler?.Invoke(mock);
        Services.Replace(new ServiceDescriptor(typeof(TService), mock.Object));
        return this;
    }

    public IServiceCollection Services { get; }
}
