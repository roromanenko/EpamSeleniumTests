using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using OpenQA.Selenium;
using Tests.TestData;

namespace Tests;

public class GlobalSearchTests : BaseTest
{
	private static class Locators
	{
		public static readonly By MagnifierIcon = By.XPath("//span[@class='search-icon dark-icon header-search__search-icon']");
		public static readonly By SearchInput = By.Id("new_form_search");
		public static readonly By FindButton = By.CssSelector("button:has(span.bth-text-layer)");
		public static By SearchResults(string keyword) =>
			By.XPath($"//div[@class='search-results__items']//a[contains(text(), '{keyword}')]");
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
		Driver!.Navigate().GoToUrl(Config.BaseUrl);
		Wait!.WaitForElementClickable(Locators.MagnifierIcon).Click();

		var searchInput = Wait.WaitForElement(Locators.SearchInput);
		searchInput.Clear();
		searchInput.SendKeys(keyword);

		Wait.WaitForElementClickable(Locators.FindButton).Click();

		var results = Driver.FindElements(Locators.SearchResults(keyword));
		results.Should().NotBeEmpty($"Expected search results should contain '{keyword}'");
		results.All(r => r.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
			.Should().BeTrue($"Expected all links to contain '{keyword}'");
	}
}
