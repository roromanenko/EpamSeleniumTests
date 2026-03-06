using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Core.Helpers;

/// <summary>
/// Provides explicit wait helpers for WebDriver element interactions.
/// All methods handle StaleElementReferenceException internally to improve stability on dynamic pages.
/// </summary>
public class WaitHelper
{
	private readonly IWebDriver _driver;
	private readonly WebDriverWait _wait;

	public WaitHelper(IWebDriver driver, int timeout)
	{
		_driver = driver;
		_wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
	}

	/// <summary>
	/// Waits until the element is present in the DOM and returns it.
	/// </summary>
	public IWebElement WaitForElement(By locator)
	{
		return _wait.Until(d =>
		{
			try
			{
				var element = d.FindElement(locator);
				_ = element.Enabled;
				return element;
			}
			catch (StaleElementReferenceException)
			{
				return null;
			}
		});
	}

	/// <summary>
	/// Waits until the element is visible and clickable, then returns it.
	/// </summary>
	public IWebElement WaitForElementClickable(By locator)
	{
		return _wait.Until(d =>
		{
			try
			{
				var element = ExpectedConditions.ElementToBeClickable(locator)(d);
				_ = element?.Enabled;
				return element;
			}
			catch (StaleElementReferenceException)
			{
				return null;
			}
		});
	}

	/// <summary>
	/// Waits until the element is visible in the DOM and returns it.
	/// </summary>
	public IWebElement WaitForElementVisible(By locator)
	{
		return _wait.Until(d =>
		{
			try
			{
				var element = ExpectedConditions.ElementIsVisible(locator)(d);
				_ = element?.Enabled;
				return element;
			}
			catch (StaleElementReferenceException)
			{
				return null;
			}
		});
	}

	/// <summary>
	/// Clicks an element using JavaScript, bypassing visibility or intercept issues.
	/// Use as a fallback when a regular click fails due to overlapping elements or animations.
	/// </summary>
	public void JsClick(By locator)
	{
		var element = WaitForElement(locator);
		((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
	}

	/// <summary>
	/// Accepts the cookie consent banner if present and waits until it disappears.
	/// </summary>
	public void HandleCookieBanner()
	{
		try
		{
			var acceptButton = _driver.FindElement(By.Id("onetrust-accept-btn-handler"));
			((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", acceptButton);
			_wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("onetrust-banner-sdk")));
		}
		catch (NoSuchElementException) { }
		catch (WebDriverTimeoutException) { }
	}
}
