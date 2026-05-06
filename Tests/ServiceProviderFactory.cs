using Core;
using Core.Configuration;
using Core.Drivers;
using Core.Interfaces;
using Core.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

public static class ServiceProviderFactory
{
	public static IServiceProvider Create()
	{
		var config = ConfigurationProvider.Config;
		LoggingBootstrap.Initialize(config.Logging);

		var services = new ServiceCollection();

		services.AddDriverFactory(drivers => drivers
			.AddChrome()
			.AddFirefox()
		);

		services.AddSingleton<ITestConfigurationProvider, ConfigurationProvider>();
		return services.BuildServiceProvider();
	}
}
