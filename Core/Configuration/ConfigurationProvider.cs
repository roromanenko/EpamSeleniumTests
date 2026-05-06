using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Configuration;

/// <summary>
/// Provides access to the test configuration loaded from appsettings.json with
/// per-environment overlay support via the TEST_ENV environment variable.
/// </summary>
public class ConfigurationProvider : ITestConfigurationProvider
{
	private const string EnvVarName = "TEST_ENV";
	private const string DefaultEnv = "dev";
	private const string EnvVarPrefix = "TAF_";

	private static readonly Lazy<TestConfiguration> _config = new(() => LoadConfiguration());

	/// <summary>
	/// Gets the singleton test configuration instance.
	/// </summary>
	public static TestConfiguration Config => _config.Value;

	public TestConfiguration GetConfiguration() => Config;

	private static TestConfiguration LoadConfiguration()
	{
		var env = Environment.GetEnvironmentVariable(EnvVarName) ?? DefaultEnv;

		var configuration = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
			.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false)
			.AddEnvironmentVariables(prefix: EnvVarPrefix)
			.Build();

		var config = configuration.Get<TestConfiguration>()
			?? throw new InvalidOperationException("Failed to load test configuration.");

		config.Environment = env;
		return config;
	}
}
