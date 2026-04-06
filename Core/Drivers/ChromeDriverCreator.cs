using Core.Interfaces;
using OpenQA.Selenium;
using SeleniumUndetectedChromeDriver;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Core.Drivers;

public class ChromeDriverCreator : IDriverCreator
{
	public ThreadLocal<IWebDriver> Create()
	{
		var driverPath = GetChromeDriverPath();
		var options = DriverOptionsFactory.CreateChromeOptions();
		return new ThreadLocal<IWebDriver>(() =>
		{
			var driver = UndetectedChromeDriver.Create(
				options: options,
				driverExecutablePath: driverPath
			);
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
