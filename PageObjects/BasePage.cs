using Core.Helpers;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace PageObjects;

public abstract class BasePage
{
	private static readonly Logger _log = LogManager.GetCurrentClassLogger();

	protected readonly IWebDriver Driver;
	protected readonly WaitHelper Wait;
	protected readonly PageSetupHelper PageSetup;

	protected BasePage(IWebDriver driver, WaitHelper wait, PageSetupHelper pageSetup)
	{
		Driver = driver;
		Wait = wait;
		PageSetup = pageSetup;
	}

	#region Navigation

	/// <summary>
	/// Navigates the browser to the specified URL.
	/// </summary>
	/// <param name="url">The full URL to navigate to.</param>
	public void NavigateTo(string url) => Driver.Navigate().GoToUrl(url);

	#endregion

	#region Interaction

	/// <summary>
	/// Clicks the element identified by the given locator.
	/// Falls back to a JavaScript click if the regular click is intercepted.
	/// </summary>
	/// <param name="locator">The Selenium locator strategy for the target element.</param>
	public void Click(By locator)
	{
		ArgumentNullException.ThrowIfNull(locator);
		_log.Info("Click: {Locator}", locator);
		try
		{
			Wait.WaitForElementClickable(locator).Click();
		}
		catch (ElementClickInterceptedException)
		{
			_log.Warn("Click intercepted on {Locator}, falling back to JS click.", locator);
			Wait.JsClick(locator);
		}
		catch (StaleElementReferenceException)
		{
			_log.Warn("Stale element on {Locator}, re-finding and retrying click.", locator);
			Wait.WaitForElementClickable(locator).Click();
		}
	}

	/// <summary>
	/// Clears the input element and types the given text into it.
	/// </summary>
	/// <param name="locator">The Selenium locator strategy for the input element.</param>
	/// <param name="text">The text to enter into the element.</param>
	public void Type(By locator, string text)
	{
		ArgumentNullException.ThrowIfNull(locator);
		_log.Info("Type: {Locator} <- \"{Text}\"", locator, text);

		var element = Wait.WaitForElement(locator);
		element.Clear();
		element.SendKeys(text);
	}

	/// <summary>
	/// Scrolls the page vertically by the given pixel offset using a wheel gesture.
	/// </summary>
	/// <param name="yOffset">Pixels to scroll; positive scrolls down, negative scrolls up.</param>
	public void ScrollByWheel(int yOffset)
	{
		_log.Info("ScrollByWheel: yOffset={YOffset}", yOffset);
		new Actions(Driver).ScrollByAmount(0, yOffset).Perform();
	}

	#endregion

	#region Wait delegates

	/// <summary>
	/// Waits until the element identified by the locator is present in the DOM and returns it.
	/// </summary>
	/// <param name="locator">The Selenium locator strategy for the target element.</param>
	/// <returns>The found <see cref="IWebElement"/>.</returns>
	public IWebElement WaitForElement(By locator)
	{
		ArgumentNullException.ThrowIfNull(locator);
		return Wait.WaitForElement(locator);
	}

	#endregion
}
