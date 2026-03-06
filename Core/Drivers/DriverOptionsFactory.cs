using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace Core.Drivers;

/// <summary>
/// Factory for creating browser-specific WebDriver options.
/// Provides pre-configured options optimized for test automation.
/// </summary>
public static class DriverOptionsFactory
{
	public static DriverOptions GetOptions(string browserName)
	{
		var browserLowerName = browserName.ToLower();
		DriverOptions options = browserLowerName switch
		{
			"chrome" => CreateChromeOptions(),
			"firefox" => CreateFirefoxOptions(),
			_ => throw new ArgumentException($"Unsupported browser: {browserName}")
		};

		return options;
	}

	#region Chrome Options

	/// <summary>
	/// Creates Chrome-specific options with settings optimized for test automation.
	/// </summary>
	public static DriverOptions CreateChromeOptions()
	{
		var options = new ChromeOptions();
		options.AddArgument("--disable-notifications");
		options.AddArgument("--incognito");
		options.AddArgument("--disable-extensions");
		options.PageLoadStrategy = PageLoadStrategy.Normal;
		return options;
	}

	#endregion

	#region Firefox Options

	/// <summary>
	/// Creates Firefox-specific options with settings optimized for test automation.
	/// </summary>
	public static DriverOptions CreateFirefoxOptions()
	{
		var options = new FirefoxOptions();
		options.AddArgument("--width=1920");
		options.AddArgument("--height=1080");
		options.AddArgument("-private");
		options.SetPreference("dom.webnotifications.enabled", false);
		options.AcceptInsecureCertificates = true;

		return options;
	}

	#endregion
}
