using NLog;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Tests.Support;

public class TestLifecycleService
{
	private static readonly Logger _log = LogManager.GetCurrentClassLogger();
	private const string ScreenshotTimestampFormat = "yyyy-MM-dd_HH-mm-ss";

	public void CaptureScreenshotOnFailure(IWebDriver driver, string testName, string screenshotDirectory, string workDirectory)
	{
		if (driver is null) return;

		var fileName = $"{SanitizeFileName(testName)}_{DateTime.Now.ToString(ScreenshotTimestampFormat)}.png";
		var dir = Path.Combine(workDirectory, screenshotDirectory);
		Directory.CreateDirectory(dir);
		var fullPath = Path.Combine(dir, fileName);
		((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(fullPath);
		TestContext.AddTestAttachment(fullPath, "Screenshot on failure");
		_log.Error("Screenshot on failure saved: {Path}", fullPath);
	}

	private static string SanitizeFileName(string name)
	{
		var invalid = Path.GetInvalidFileNameChars();
		return string.Concat(name.Select(c => invalid.Contains(c) ? '_' : c));
	}
}
