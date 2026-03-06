using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Core.Helpers;

public class WaitHelper
{
	private readonly IWebDriver _driver;
	private readonly WebDriverWait _wait;

	public WaitHelper(IWebDriver driver, int timeout)
	{
		_driver = driver;
		_wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
	}

	public IWebElement WaitForElement(By locator)
	{
		return _wait.Until(d =>
		{
			try
			{
				var element = d.FindElement(locator);
				_ = element.Enabled; // probe for staleness
				return element;
			}
			catch (StaleElementReferenceException)
			{
				return null;
			}
		});
	}

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

	public void JsClick(By locator)
	{
		var element = WaitForElement(locator);
		((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
	}

	public void AcceptCookiesIfPresent()
	{
		try
		{
			var acceptButton = _driver.FindElement(By.Id("onetrust-accept-btn-handler"));
			((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", acceptButton);
		}
		catch (NoSuchElementException) { }
	}
	public void WaitForCookieBannerToDisappear()
	{
		try
		{
			_wait.Until(ExpectedConditions.InvisibilityOfElementLocated(
				By.Id("onetrust-banner-sdk")));
		}
		catch (WebDriverTimeoutException) { }
	}
}
