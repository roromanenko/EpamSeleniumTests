using System.Collections.Concurrent;
using Core.Interfaces;
using NLog;
using OpenQA.Selenium;

namespace Core.Drivers;

/// <summary>
/// Browser Factory: creates and manages WebDriver instances per browser and per thread.
/// </summary>
public class DriverFactory : IWebDriverFactory
{
	private static readonly Logger _log = LogManager.GetCurrentClassLogger();

	private readonly ConcurrentDictionary<string, Lazy<ThreadLocal<IWebDriver>>> _drivers = new();
	private readonly Dictionary<string, IDriverCreator> _creators;

	public DriverFactory(Dictionary<string, IDriverCreator> creators)
	{
		_creators = creators;
	}

	public ThreadLocal<IWebDriver> GetDriver(string browserName)
	{
		return _drivers.GetOrAdd(browserName, name =>
		{
			if (!_creators.TryGetValue(name.ToLower(), out var creator))
				throw new ArgumentException($"Unsupported browser: {name}");

			_log.Info("GetDriver: {Browser}", name);

			return new Lazy<ThreadLocal<IWebDriver>>(
				creator.Create,
				LazyThreadSafetyMode.ExecutionAndPublication
			);
		}).Value;
	}

	/// <summary>
	/// Quits and disposes all driver instances for the specified browser across all threads.
	/// </summary>
	public void QuitDriver(string browserName)
	{
		_log.Info("QuitDriver: {Browser}", browserName);

		if (_drivers.TryGetValue(browserName, out var webDriver))
		{
			if (webDriver.IsValueCreated)
			{
				if (webDriver.Value.IsValueCreated)
				{
					webDriver.Value.Value?.Quit();
					webDriver.Value.Value?.Dispose();
				}
				webDriver.Value.Dispose();
			}

			_drivers.TryRemove(browserName, out _);
		}
	}
}
