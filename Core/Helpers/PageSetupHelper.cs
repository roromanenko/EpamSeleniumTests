using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Core.Helpers;

/// <summary>
/// Provides helper methods for initial page setup before test execution.
/// </summary>
public class PageSetupHelper
{
	private static class Locators
	{
		public static readonly By CookieAcceptButton = By.Id("onetrust-accept-btn-handler");
		public static readonly By CookieBanner = By.Id("onetrust-banner-sdk");
	}

	private readonly IWebDriver _driver;
	private readonly WebDriverWait _wait;

	public PageSetupHelper(IWebDriver driver, int timeout)
	{
		_driver = driver;
		_wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
	}

	/// <summary>
	/// Accepts the cookie consent banner if present and waits until it disappears.
	/// </summary>
	public void HandleCookieBanner()
	{
		try
		{
			var acceptButton = _driver.FindElement(Locators.CookieAcceptButton);
			((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", acceptButton);
			_wait.Until(ExpectedConditions.InvisibilityOfElementLocated(Locators.CookieBanner));
		}
		catch (NoSuchElementException) { }
		catch (WebDriverTimeoutException) { }
	}
}
