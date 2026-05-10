using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Microsoft.Extensions.DependencyInjection;

namespace Tests.Support;

public static class ReqnrollServiceRegistration
{
	[ScenarioDependencies]
	public static IServiceCollection CreateServices()
	{
		var services = new ServiceCollection();
		ServiceProviderFactory.ConfigureServices(services);
		services.AddScoped<ScenarioState>();
		return services;
	}
}
