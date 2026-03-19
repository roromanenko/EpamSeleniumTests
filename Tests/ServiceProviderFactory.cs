using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Core.Configuration;
using Core.Drivers;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

public static class ServiceProviderFactory
{
	public static IServiceProvider Create()
	{
		var services = new ServiceCollection();

		services.AddDriverFactory(drivers => drivers
			.AddChrome()
			.AddFirefox()
		);

		services.AddSingleton<ITestConfigurationProvider, ConfigurationProvider>();
		return services.BuildServiceProvider();
	}
}
