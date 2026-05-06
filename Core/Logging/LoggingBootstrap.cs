using Core.Configuration;
using NLog;

namespace Core.Logging;

/// <summary>
/// Entry point for NLog initialisation. Must be called once before any logger is used.
/// </summary>
public static class LoggingBootstrap
{
	private const string MinLevelVariable = "minLevel";
	private const string LogDirVariable = "logDir";

	/// <summary>
	/// Configures NLog variables from <paramref name="loggingConfig"/> and reloads all
	/// existing loggers so they pick up the new settings.
	/// </summary>
	public static void Initialize(LoggingConfiguration loggingConfig)
	{
		ArgumentNullException.ThrowIfNull(loggingConfig);

		var nlogConfig = LogManager.Configuration
			?? throw new InvalidOperationException("nlog.config was not found or could not be loaded.");

		nlogConfig.Variables[MinLevelVariable] = loggingConfig.MinLevel;
		nlogConfig.Variables[LogDirVariable] = loggingConfig.FileLogDirectory;

		// Reconfigure existing loggers so any logger obtained before Initialize is called
		// (e.g. static-field loggers initialised at type load time) picks up the updated
		// variable values immediately.
		LogManager.ReconfigExistingLoggers();
	}
}
