using System.Collections.Concurrent;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using SeleniumUndetectedChromeDriver;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Core.Drivers;

public sealed class DriverFactory
{
	private static readonly ConcurrentDictionary<string, Lazy<ThreadLocal<IWebDriver>>> _drivers = new();

	private DriverFactory() { }

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

	private static ThreadLocal<IWebDriver> CreateChromeDriver(ChromeOptions options)
	{
		var driverExecutablePath = new DriverManager().SetUpDriver(new ChromeConfig(), "145.0.7632.160");
		return new ThreadLocal<IWebDriver>(() =>
			UndetectedChromeDriver.Create(
				options: options,
				driverExecutablePath: driverExecutablePath
			));
	}

	private static ThreadLocal<IWebDriver> CreateFirefoxDriver(FirefoxOptions options)
	{
		new DriverManager().SetUpDriver(new FirefoxConfig());
		return new ThreadLocal<IWebDriver>(() => new FirefoxDriver(options));
	}

	#endregion

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

	public static bool IsDriverInitialized(string browserName)
	{
		return _drivers.TryGetValue(browserName, out var webDriver)
			&& webDriver.IsValueCreated
			&& webDriver.Value.IsValueCreated;
	}
}
