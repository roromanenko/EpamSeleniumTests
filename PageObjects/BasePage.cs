using Core.Helpers;
using OpenQA.Selenium;

namespace PageObjects;

public abstract class BasePage
{
	protected readonly IWebDriver Driver;
	protected readonly WaitHelper Wait;

	protected BasePage(IWebDriver driver, WaitHelper wait)
	{
		Driver = driver;
		Wait = wait;
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
		Log($"Click: {locator}");
		try
		{
			Wait.WaitForElementClickable(locator).Click();
		}
		catch (ElementClickInterceptedException)
		{
			Log($"WARNING: Click intercepted on {locator}, falling back to JS click");
			Wait.JsClick(locator);
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
		Log($"Type: {locator} <- \"{text}\"");

		var element = Wait.WaitForElement(locator);
		element.Clear();
		element.SendKeys(text);

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

	#region Private helpers

	private void Log(string message) => Console.WriteLine($"[BasePage] {message}");

	#endregion
}
