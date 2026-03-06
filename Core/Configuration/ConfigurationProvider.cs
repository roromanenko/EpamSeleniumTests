using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Core.Configuration;

public class ConfigurationProvider
{
	private static readonly Lazy<TestConfiguration> _config = new(() => LoadConfiguration());

	public static TestConfiguration Config => _config.Value;

	private static TestConfiguration LoadConfiguration()
	{
		var configuration = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
			.Build();

		return configuration.Get<TestConfiguration>()
			?? throw new InvalidOperationException("Failed to load test configuration.");
	}
}
