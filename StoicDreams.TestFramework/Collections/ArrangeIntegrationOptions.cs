using Microsoft.Extensions.DependencyInjection.Extensions;

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

    public IServiceCollection Services { get; }
}
