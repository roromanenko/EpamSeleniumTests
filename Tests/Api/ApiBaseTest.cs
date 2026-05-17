using Core.Api;
using Core.Configuration;
using NLog;

namespace Tests.Api;

/// <summary>
/// Abstract base class for API test fixtures.
/// Provides a per-test <see cref="RestApiClient"/> instance and SetUp logging.
/// No WebDriver, DI scope, or screenshot hook — separate from <see cref="BaseTest"/>.
/// </summary>
public abstract class ApiBaseTest
{
    private static readonly Logger _log = LogManager.GetCurrentClassLogger();

    protected RestApiClient ApiClient { get; private set; } = null!;

    [SetUp]
    public void ApiSetUp()
    {
        ApiClient = new RestApiClient(ConfigurationProvider.Config.Api);
        _log.Info("SetUp: {TestName} (env={Env}, apiBaseUrl={BaseUrl})",
            TestContext.CurrentContext.Test.Name,
            ConfigurationProvider.Config.Environment,
            ConfigurationProvider.Config.Api.BaseUrl);
    }
}
