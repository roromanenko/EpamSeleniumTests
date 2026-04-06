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
	/// Core wait method. Handles StaleElementReferenceException for all wait operations.
	/// </summary>
	private IWebElement WaitForElement(Func<IWebDriver, IWebElement?> condition)
	{
		return _wait.Until(d =>
		{
			try
			{
				var element = condition(d);
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
	/// Waits until the element is present in the DOM and returns it.
	/// </summary>
	public IWebElement WaitForElement(By locator) =>
		WaitForElement(d => d.FindElement(locator));

	/// <summary>
	/// Waits until the element is visible and clickable, then returns it.
	/// </summary>
	public IWebElement WaitForElementClickable(By locator) =>
		WaitForElement(d => ExpectedConditions.ElementToBeClickable(locator)(d));

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
	/// Scrolls the element into the visible area of the browser window.
	/// </summary>
	public void ScrollIntoView(By locator)
	{
		var element = WaitForElement(locator);
		((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
	}

	/// <summary>
	/// Waits until at least one element matching the locator is present and returns all matches.
	/// </summary>
	public IReadOnlyCollection<IWebElement> WaitForElements(By locator)
	{
		return _wait.Until(d =>
		{
			var elements = d.FindElements(locator);
			return elements.Count > 0 ? elements : null;
		});
	}
}
