using Core.Configuration;
using Core.Helpers;
using Core.Interfaces;
using NLog;
using NUnit.Framework;
using Reqnroll;
using Tests.Support;

namespace Tests.Hooks;

[Binding]
public class ScenarioHooks
{
	private static readonly Logger _log = LogManager.GetCurrentClassLogger();
	private const string BrowserTagPrefix = "@browser:";

	private readonly IWebDriverFactory _driverFactory;
	private readonly TestConfiguration _config;
	private readonly ScenarioState _state;
	private readonly TestLifecycleService _lifecycle;

	public ScenarioHooks(IWebDriverFactory driverFactory, ITestConfigurationProvider configProvider, ScenarioState state, TestLifecycleService lifecycle)
	{
		_driverFactory = driverFactory;
		_config = configProvider.GetConfiguration();
		_state = state;
		_lifecycle = lifecycle;
	}

	[BeforeScenario(Order = 0)]
	public void BeforeScenario(ScenarioContext scenarioContext)
	{
		_state.BrowserKey = ResolveBrowserKey(scenarioContext);
		_state.Driver = _driverFactory.GetDriver(_state.BrowserKey).Value
			?? throw new InvalidOperationException("WebDriver instance is null.");
		_state.Wait = new WaitHelper(_state.Driver, _config.Timeouts.ExplicitWait);
		_state.PageSetup = new PageSetupHelper(_state.Driver, _config.Timeouts.ExplicitWait);
		_state.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_config.Timeouts.ImplicitWait);
		_log.Info("BeforeScenario: {Title} (env={Env})", scenarioContext.ScenarioInfo.Title, _config.Environment);
	}

	[AfterScenario(Order = 0)]
	public void AfterScenario(ScenarioContext scenarioContext)
	{
		try
		{
			if (scenarioContext.TestError is not null)
				_lifecycle.CaptureScreenshotOnFailure(_state.Driver, scenarioContext.ScenarioInfo.Title, _config.Screenshots.Directory, TestContext.CurrentContext.WorkDirectory);
		}
		finally
		{
			_log.Info("AfterScenario: {Title}", scenarioContext.ScenarioInfo.Title);
			_driverFactory.QuitDriver(_state.BrowserKey);
		}
	}

	private string ResolveBrowserKey(ScenarioContext scenarioContext)
	{
		var browserTag = scenarioContext.ScenarioInfo.Tags
			.FirstOrDefault(t => t.StartsWith(BrowserTagPrefix, StringComparison.OrdinalIgnoreCase));
		return browserTag is not null
			? browserTag[BrowserTagPrefix.Length..]
			: _config.Browser;
	}
}
