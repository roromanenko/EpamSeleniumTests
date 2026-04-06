using Core.Configuration;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;

namespace Tests;

/// <summary>
/// Base class for all test fixtures.
/// Handles WebDriver initialization and cleanup for each test.
/// </summary>
[TestFixture]
public abstract class BaseTest
{
	private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.Create();

	protected readonly IWebDriverFactory DriverFactory =
		_serviceProvider.GetRequiredService<IWebDriverFactory>();
	protected TestConfiguration Config =
		_serviceProvider.GetRequiredService<ITestConfigurationProvider>().GetConfiguration();

	protected IWebDriver Driver { get; private set; }
	protected WaitHelper Wait { get; private set; }
	protected PageSetupHelper PageSetup { get; private set; } = null!;

	public BaseTest()
	{
	}

	[SetUp]
	public void SetUp()
	{
		Driver = DriverFactory.GetDriver(Config.Browser).Value
			?? throw new InvalidOperationException("WebDriver instance is null.");
		Wait = new WaitHelper(Driver, Config.Timeouts.ExplicitWait);
		PageSetup = new PageSetupHelper(Driver, Config.Timeouts.ExplicitWait);
		Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.Timeouts.ImplicitWait);
	}

	[TearDown]
	public void TearDown()
	{
		DriverFactory.QuitDriver(Config.Browser);
	}
}
