using System.Collections.Concurrent;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using SeleniumUndetectedChromeDriver;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Core.Drivers;

/// <summary>
/// Factory for creating and managing WebDriver instances per browser and thread.
/// </summary>
public static class DriverFactory
{
	private static readonly ConcurrentDictionary<string, Lazy<ThreadLocal<IWebDriver>>> _drivers = new();

	/// <summary>
	/// Gets the singleton WebDriver instance.
	/// Creates a new instance if one doesn't exist.
	/// </summary>
	/// <returns>The singleton WebDriver instance</returns>
	public static ThreadLocal<IWebDriver> GetDriver(string browserName)
	{
		return _drivers.GetOrAdd(browserName, CreateDriver).Value;
	}

	#region Create Driver

	private static Lazy<ThreadLocal<IWebDriver>> CreateDriver(string browserName)
	{
		DriverOptions options = DriverOptionsFactory.GetOptions(browserName);

		return browserName.ToLower() switch
		{
			"chrome" => new Lazy<ThreadLocal<IWebDriver>>(() => CreateChromeDriver((ChromeOptions)options), LazyThreadSafetyMode.ExecutionAndPublication),
			"firefox" => new Lazy<ThreadLocal<IWebDriver>>(() => CreateFirefoxDriver((FirefoxOptions)options), LazyThreadSafetyMode.ExecutionAndPublication),
			_ => throw new ArgumentException($"Unsupported browser: {browserName}")
		};
	}

	/// <summary>
	/// Creates a Chrome WebDriver instance using UndetectedChromeDriver to bypass Cloudflare bot protection.
	/// Uses GetMatchingBrowserVersion() to resolve ChromeDriver/Chrome version mismatch.
	/// See teams message for more details.
	/// </summary>
	private static ThreadLocal<IWebDriver> CreateChromeDriver(ChromeOptions options)
	{
		var driverPath = GetChromeDriverPath();

		return new ThreadLocal<IWebDriver>(() =>
			UndetectedChromeDriver.Create(
				options: options,
				driverExecutablePath: driverPath
			));
	}

	private static string GetChromeDriverPath()
	{
		var config = new ChromeConfig();
		var version = config.GetMatchingBrowserVersion();

		return new DriverManager().SetUpDriver(config, version);
	}

	private static ThreadLocal<IWebDriver> CreateFirefoxDriver(FirefoxOptions options)
	{
		new DriverManager().SetUpDriver(new FirefoxConfig());
		return new ThreadLocal<IWebDriver>(() => new FirefoxDriver(options));
	}

	#endregion

	/// <summary>
	/// Quits and disposes all driver instances for the specified browser across all threads.
	/// </summary>
	public static void QuitDriver(string browserName)
	{
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

	/// <summary>
	/// Quits and disposes the driver for the current thread only
	/// </summary>
	public static void QuitDriverForCurrentThread(string browserName)
	{
		if (_drivers.TryGetValue(browserName, out var webDriver))
		{
			if (webDriver.IsValueCreated && webDriver.Value.IsValueCreated)
			{
				webDriver.Value.Value?.Quit();
				webDriver.Value.Value?.Dispose();
			}
		}
	}
}
