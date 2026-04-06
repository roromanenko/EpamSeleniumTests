using Core.Helpers;
using OpenQA.Selenium;

namespace PageObjects;

public class GlobalSearchPage : BasePage
{
	#region Locators

	private static readonly By _searchInput = By.Id("new_form_search");

	private static readonly By _findButton = By.CssSelector("button:has(span.bth-text-layer)");

	private static By SearchResultLinks(string keyword) => By.XPath($"//div[@class='search-results__items']//a[contains(text(), '{keyword}')]");

	#endregion

	public GlobalSearchPage(IWebDriver driver, WaitHelper wait, PageSetupHelper pageSetup) : base(driver, wait, pageSetup) { }

	#region Actions

	/// <summary>
	/// Types the keyword into the search input and clicks the Find button.
	/// </summary>
	/// <param name="keyword">The search term to submit.</param>
	public void SearchFor(string keyword)
	{
		Type(_searchInput, keyword);
		Click(_findButton);
	}

	/// <summary>
	/// Returns all result link elements whose text contains the given keyword.
	/// </summary>
	/// <param name="keyword">The keyword to filter result links by.</param>
	/// <returns>A read-only collection of matching <see cref="IWebElement"/> links.</returns>
	public IReadOnlyCollection<IWebElement> GetResultLinks(string keyword) =>
		Driver.FindElements(SearchResultLinks(keyword));

	#endregion
}
