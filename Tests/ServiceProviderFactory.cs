using Core;
using Core.Configuration;
using Core.Drivers;
using Core.Interfaces;
using Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Tests.Support;

namespace Tests;

public static class ServiceProviderFactory
{
	public static IServiceProvider Create()
	{
		var services = new ServiceCollection();
		ConfigureServices(services);
		return services.BuildServiceProvider();
	}

	public static void ConfigureServices(IServiceCollection services)
	{
		var config = ConfigurationProvider.Config;
		LoggingBootstrap.Initialize(config.Logging);

		services.AddDriverFactory(drivers => drivers
			.AddChrome()
			.AddFirefox()
		);

		services.AddSingleton<ITestConfigurationProvider, ConfigurationProvider>();
		services.AddScoped<TestLifecycleService>();
	}
}
