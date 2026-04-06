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
}

/// <summary>
/// Represents timeout settings for WebDriver waits.
/// </summary>
public class TimeoutsConfiguration
{
	public int ImplicitWait { get; set; }
	public int ExplicitWait { get; set; }
}