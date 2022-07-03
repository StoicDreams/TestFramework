using Microsoft.Extensions.DependencyInjection;

namespace SampleProject;

public static class SampleStartup
{
	public static IServiceCollection ConfigureServices(IServiceCollection services)
	{
		services.AddTransient<ISampleParent, SampleParent>();
		services.AddTransient<ISampleChildA, SampleChildA>();
		services.AddTransient<ISampleChildB, SampleChildB>();
		return services;
	}
}
