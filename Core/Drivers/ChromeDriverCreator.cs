using Core.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumUndetectedChromeDriver;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Core.Drivers;

public class ChromeDriverCreator : IDriverCreator
{
	private static readonly string _downloadDirectory =
		Path.GetFullPath(Path.Combine(Path.GetTempPath(), Configuration.ConfigurationProvider.Config.DownloadDirectory));

	public ThreadLocal<IWebDriver> Create()
	{
		Directory.CreateDirectory(_downloadDirectory);
		var driverPath = GetChromeDriverPath();
		var options = DriverOptionsFactory.CreateChromeOptions(_downloadDirectory);
		return new ThreadLocal<IWebDriver>(() =>
		{
			var driver = UndetectedChromeDriver.Create(
				options: options,
				driverExecutablePath: driverPath
			);
			((ChromeDriver)driver).ExecuteCdpCommand("Browser.setDownloadBehavior", new Dictionary<string, object?>
			{
				["behavior"] = "allow",
				["downloadPath"] = _downloadDirectory
			});
			driver.Manage().Window.Maximize();
			return driver;
		});
	}

	private static string GetChromeDriverPath()
	{
		var config = new ChromeConfig();
		var version = config.GetMatchingBrowserVersion();
		return new DriverManager().SetUpDriver(config, version);
	}
}
