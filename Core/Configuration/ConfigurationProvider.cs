using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Configuration;

/// <summary>
/// Provides access to the test configuration loaded from appsettings.json.
/// </summary>
public class ConfigurationProvider : ITestConfigurationProvider
{
	private static readonly Lazy<TestConfiguration> _config = new(() => LoadConfiguration());

	/// <summary>
	/// Gets the singleton test configuration instance.
	/// </summary>
	public static TestConfiguration Config => _config.Value;

	public TestConfiguration GetConfiguration() => Config;

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
