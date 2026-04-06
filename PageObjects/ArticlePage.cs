using Core.Helpers;
using OpenQA.Selenium;

namespace PageObjects;

public class ArticlePage : BasePage
{
	#region Locators

	private static readonly By _articleTitle = By.CssSelector("h1.scaling-of-text-wrapper");

	#endregion

	public ArticlePage(IWebDriver driver, WaitHelper wait, PageSetupHelper pageSetup) : base(driver, wait, pageSetup) { }

	#region Actions

	/// <summary>
	/// Waits for the article heading element and returns its text.
	/// </summary>
	public string GetArticleTitle()
		=> Wait.WaitForNonEmptyText(_articleTitle);

	#endregion
}
