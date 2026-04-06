using FluentAssertions;
using PageObjects;
using Tests.TestData;

namespace Tests;

public class GlobalSearchTests : BaseTest
{
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
		var homePage = new HomePage(Driver!, Wait!, PageSetup);
		homePage.NavigateTo(Config.BaseUrl);

		var searchPage = homePage.OpenSearch();
		searchPage.SearchFor(keyword);

		var results = searchPage.GetResultLinks(keyword);
		results.Should().NotBeEmpty($"Expected search results should contain '{keyword}'");
		results.All(r => r.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
			.Should().BeTrue($"Expected all links to contain '{keyword}'");
	}
}
