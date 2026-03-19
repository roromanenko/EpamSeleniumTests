using Core.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Core.Drivers;

public class FirefoxDriverCreator : IDriverCreator
{
	public ThreadLocal<IWebDriver> Create()
	{
		var options = DriverOptionsFactory.CreateFirefoxOptions();
		new DriverManager().SetUpDriver(new FirefoxConfig());
		return new ThreadLocal<IWebDriver>(() => new FirefoxDriver(options));
	}
}
