using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace Core.Drivers;

/// <summary>
/// Factory for creating browser-specific WebDriver options.
/// Uses Registry Pattern — open for extension without modifying the class.
/// </summary>
public static class DriverOptionsFactory
{
	#region Chrome Options

	/// <summary>
	/// Creates Chrome-specific options with settings optimized for test automation.
	/// </summary>
	public static ChromeOptions CreateChromeOptions()
	{
		var options = new ChromeOptions();
		options.AddArgument("--disable-notifications");
		options.AddArgument("--incognito");
		options.AddArgument("--disable-extensions");
		options.AddArgument("--start-maximized");
		options.PageLoadStrategy = PageLoadStrategy.Normal;
		return options;
	}

	#endregion

	#region Firefox Options

	/// <summary>
	/// Creates Firefox-specific options with settings optimized for test automation.
	/// </summary>
	public static FirefoxOptions CreateFirefoxOptions()
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
