using Core.Configuration;
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
	private const string ChromeHeadlessArg = "--headless=new";
	// --no-sandbox is required when Chrome runs as root in Linux CI containers
	// that lack user-namespace sandbox capabilities (e.g. GitHub Actions ubuntu-latest).
	private const string ChromeNoSandboxArg = "--no-sandbox";
	// --start-maximized is ignored in headless mode, so an explicit window size
	// is required — otherwise Chrome defaults to ~800x600 and sites render their mobile layout.
	private const string ChromeHeadlessWindowSizeArg = "--window-size=1920,1080";
	private const string FirefoxHeadlessArg = "-headless";

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

		if (ConfigurationProvider.Config.Headless)
		{
			options.AddArgument(ChromeHeadlessArg);
			options.AddArgument(ChromeNoSandboxArg);
			options.AddArgument(ChromeHeadlessWindowSizeArg);
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

		if (ConfigurationProvider.Config.Headless)
			options.AddArgument(FirefoxHeadlessArg);

		return options;
	}

	#endregion
}
