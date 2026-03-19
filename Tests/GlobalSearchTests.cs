using Core.Configuration;
using Core.Drivers;
using FluentAssertions;
using OpenQA.Selenium;
using Tests.TestData;

namespace Tests;

public class GlobalSearchTests : BaseTest
{
	private static class Locators
	{
		public static readonly By MagnifierIcon = By.ClassName("search-icon");
		public static readonly By SearchInput = By.TagName("input");
		public static readonly By FindButton = By.CssSelector("button:has(span.bth-text-layer)");
		public static By SearchResultByKeyword(string keyword) =>
			By.PartialLinkText(keyword);
	}

	/// <summary>
	/// TC-2: Validate global search works as expected<br/>
	/// 1. Navigate to https://www.epam.com/<br/>
	/// 2. Find a magnifier icon and click on it<br/>
	/// 3. Type a search string in the search field<br/>
	/// 4. Click "Find" button<br/>
	/// 5. Validate that all links in the list contain the search word
	/// </summary>
	[TestCaseSource(typeof(SearchTestData), nameof(SearchTestData.GlobalSearchData))]
	public void ValidateGlobalSearchWorksAsExpected(string keyword)
	{
		Driver.Navigate().GoToUrl(Config.BaseUrl);
		Wait.WaitForElementClickable(Locators.MagnifierIcon).Click();

		var searchInput = Wait.WaitForElement(Locators.SearchInput);
		searchInput.Clear();
		searchInput.SendKeys(keyword);

		Wait.WaitForElementClickable(Locators.FindButton).Click();

		var results = Wait.WaitForElements(Locators.SearchResultByKeyword(keyword));
		results.Should().NotBeEmpty($"Expected search results should contain '{keyword}'");
		results.All(r => r.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
			.Should().BeTrue($"Expected all links to contain '{keyword}'");
	}
}
