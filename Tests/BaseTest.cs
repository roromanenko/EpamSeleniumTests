using Core.Configuration;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace Tests;

/// <summary>
/// Base class for all test fixtures.
/// Handles WebDriver initialization, cleanup, and screenshot-on-failure for each test.
/// </summary>
[TestFixture]
public abstract class BaseTest
{
	private static readonly Logger _log = LogManager.GetCurrentClassLogger();
	private const string ScreenshotTimestampFormat = "yyyy-MM-dd_HH-mm-ss";

	private static readonly IServiceProvider _serviceProvider = ServiceProviderFactory.Create();

	protected readonly IWebDriverFactory DriverFactory =
		_serviceProvider.GetRequiredService<IWebDriverFactory>();
	protected readonly TestConfiguration Config =
		_serviceProvider.GetRequiredService<ITestConfigurationProvider>().GetConfiguration();

	protected IWebDriver Driver { get; private set; } = null!;
	protected WaitHelper Wait { get; private set; } = null!;
	protected PageSetupHelper PageSetup { get; private set; } = null!;

	protected virtual string BrowserKey => Config.Browser;

	[SetUp]
	public void SetUp()
	{
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
				CaptureScreenshotOnFailure();
		}
		finally
		{
			_log.Info("TearDown: {TestName}", TestContext.CurrentContext.Test.Name);
			DriverFactory.QuitDriver(BrowserKey);
		}
	}

	private void CaptureScreenshotOnFailure()
	{
		if (Driver is null) return;

		var safeName = SanitizeFileName(TestContext.CurrentContext.Test.Name);
		var fileName = $"{safeName}_{DateTime.Now.ToString(ScreenshotTimestampFormat)}.png";
		var dir = Path.Combine(TestContext.CurrentContext.WorkDirectory, Config.Screenshots.Directory);
		Directory.CreateDirectory(dir);
		var fullPath = Path.Combine(dir, fileName);
		((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(fullPath);
		TestContext.AddTestAttachment(fullPath, "Screenshot on failure");
		_log.Error("Screenshot on failure saved: {Path}", fullPath);
	}

	private static string SanitizeFileName(string name)
	{
		var invalid = Path.GetInvalidFileNameChars();
		return string.Concat(name.Select(c => invalid.Contains(c) ? '_' : c));
	}
}
