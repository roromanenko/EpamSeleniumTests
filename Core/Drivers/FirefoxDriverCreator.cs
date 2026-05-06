using Core.Interfaces;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Core.Drivers;

public class FirefoxDriverCreator : IDriverCreator
{
	private static readonly Logger _log = LogManager.GetCurrentClassLogger();

	public ThreadLocal<IWebDriver> Create()
	{
		var options = DriverOptionsFactory.CreateFirefoxOptions();
		new DriverManager().SetUpDriver(new FirefoxConfig());
		return new ThreadLocal<IWebDriver>(() =>
		{
			_log.Info("FirefoxDriver created.");
			return new FirefoxDriver(options);
		});
	}
}
