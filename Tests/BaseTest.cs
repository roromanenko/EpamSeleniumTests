using Core.Configuration;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using Tests.Support;

namespace Tests;

/// <summary>
/// Base class for all test fixtures.
/// Handles WebDriver initialization, cleanup, and screenshot-on-failure for each test.
/// </summary>
[TestFixture]
public abstract class BaseTest
{
	private static readonly Logger _log = LogManager.GetCurrentClassLogger();

	private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.Create();

	protected readonly IWebDriverFactory DriverFactory =
		_serviceProvider.GetRequiredService<IWebDriverFactory>();
	protected readonly TestConfiguration Config =
		_serviceProvider.GetRequiredService<ITestConfigurationProvider>().GetConfiguration();

	private IServiceScope _scope = null!;
	private TestLifecycleService _lifecycle = null!;

	protected IWebDriver Driver { get; private set; } = null!;
	protected WaitHelper Wait { get; private set; } = null!;
	protected PageSetupHelper PageSetup { get; private set; } = null!;

	protected virtual string BrowserKey => Config.Browser;

	[SetUp]
	public void SetUp()
	{
		_scope = _serviceProvider.CreateScope();
		_lifecycle = _scope.ServiceProvider.GetRequiredService<TestLifecycleService>();

		Driver = DriverFactory.GetDriver(BrowserKey).Value
			?? throw new InvalidOperationException("WebDriver instance is null.");
		Wait = new WaitHelper(Driver, Config.Timeouts.ExplicitWait);
		PageSetup = new PageSetupHelper(Driver, Config.Timeouts.ExplicitWait);
		Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.Timeouts.ImplicitWait);
		_log.Info("SetUp: {TestName} (env={Env})", TestContext.CurrentContext.Test.Name, Config.Environment);
	}

	[TearDown]
	public void TearDown()
	{
		try
		{
			if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
				_lifecycle.CaptureScreenshotOnFailure(Driver, TestContext.CurrentContext.Test.Name, Config.Screenshots.Directory, TestContext.CurrentContext.WorkDirectory);
		}
		finally
		{
			_log.Info("TearDown: {TestName}", TestContext.CurrentContext.Test.Name);
			DriverFactory.QuitDriver(BrowserKey);
			_scope?.Dispose();
		}
	}
}
