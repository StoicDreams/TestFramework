using Microsoft.Extensions.DependencyInjection;
using SampleBlazorLib.Data;
using SampleBlazorLib.Interfaces;

namespace SampleBlazorLib;

internal static class SampleStartup
{
	public static IServiceCollection Startup(IServiceCollection services)
	{
		services.AddSingleton<ICache, Cache>();
		return services;
	}
}
