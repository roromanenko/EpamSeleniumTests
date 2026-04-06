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
	/// <param name="downloadDirectory">
	/// Optional path for the Chrome download directory.
	/// When non-null, Chrome is configured to save files there without prompting.
	/// </param>
	public static ChromeOptions CreateChromeOptions(string? downloadDirectory = null)
	{
		var options = new ChromeOptions();
		options.AddArgument("--disable-notifications");
		options.AddArgument("--disable-extensions");
		options.AddArgument("--start-maximized");
		options.PageLoadStrategy = PageLoadStrategy.Normal;

		if (downloadDirectory is null)
		{
			options.AddArgument("--incognito");
		}
		else
		{
			options.AddArgument("--safebrowsing-disable-download-protection");
			options.AddArgument("--disable-features=DownloadBubble,DownloadBubbleV2");
		}

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
