namespace Core.Configuration;

/// <summary>
/// Represents the root test configuration loaded from appsettings.json.
/// </summary>
public class TestConfiguration
{
	public required string Browser { get; set; }
	public required string BaseUrl { get; set; }
	public required TimeoutsConfiguration Timeouts { get; set; }
	public required string DownloadDirectory { get; set; }
	public required LoggingConfiguration Logging { get; set; }
	public required ScreenshotsConfiguration Screenshots { get; set; }
	public required ApiConfiguration Api { get; set; }
	public string Environment { get; set; } = string.Empty;
}

/// <summary>
/// Represents timeout settings for WebDriver waits.
/// </summary>
public class TimeoutsConfiguration
{
	public int ImplicitWait { get; set; }
	public int ExplicitWait { get; set; }
}

/// <summary>
/// Represents NLog logging configuration for the test framework.
/// </summary>
public class LoggingConfiguration
{
	public required string MinLevel { get; set; }
	public required string FileLogDirectory { get; set; }
}

/// <summary>
/// Represents screenshot output configuration for failure captures.
/// </summary>
public class ScreenshotsConfiguration
{
	public required string Directory { get; set; }
}

/// <summary>
/// Represents HTTP API client configuration for test fixtures.
/// </summary>
public class ApiConfiguration
{
	public required string BaseUrl { get; set; }
	public required int Timeout { get; set; }
}