using Core.Configuration;
using Core.Drivers;
using Core.Helpers;
using OpenQA.Selenium;

namespace Tests;

[TestFixture]
public abstract class BaseTest
{
	protected IWebDriver? Driver { get; private set; }
	protected WaitHelper? Wait { get; private set; }
	protected static readonly TestConfiguration Config = ConfigurationProvider.Config;

	[SetUp]
	public void SetUp()
	{
		Driver = DriverFactory.GetDriver(Config.Browser).Value;
		Wait = new WaitHelper(Driver!, Config.Timeouts.ExplicitWait);

		Driver!.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.Timeouts.ImplicitWait);

	}

	[TearDown]
	public void TearDown()
	{
		DriverFactory.QuitDriver(Config.Browser);
	}
}
