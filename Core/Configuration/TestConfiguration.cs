namespace Core.Configuration;

public class TestConfiguration
{
	public required string Browser { get; set; }
	public required string BaseUrl { get; set; }
	public required TimeoutsConfiguration Timeouts { get; set; }
}

public class TimeoutsConfiguration
{
	public int ImplicitWait { get; set; }
	public int ExplicitWait { get; set; }
}