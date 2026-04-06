using FluentAssertions;
using PageObjects;
using Tests.TestData;

namespace Tests;

public class CarouselArticleTitleTests : BaseTest
{
	/// <summary>
	/// TC: Validate that the article title shown in the Insights carousel is a substring
	/// of the full title on the article detail page reached by clicking "Read More".<br/>
	/// 1. Navigate to https://www.epam.com/<br/>
	/// 2. Handle the cookie banner<br/>
	/// 3. Click the "Insights" navigation link<br/>
	/// 4. Swipe the carousel right <paramref name="swipeCount"/> times<br/>
	/// 5. Capture the active carousel article title<br/>
	/// 6. Click "Read More" to open the article detail page<br/>
	/// 7. Validate that the article page title contains the carousel title
	/// </summary>
	[TestCaseSource(typeof(CarouselTestData), nameof(CarouselTestData.SwipeCountData))]
	public void ValidateCarouselArticleTitleMatchesArticlePage(int swipeCount)
	{
		var homePage = new HomePage(Driver!, Wait!, PageSetup);
		homePage.NavigateTo(Config.BaseUrl);
		PageSetup.HandleCookieBanner();

		var insightsPage = homePage.GoToInsights();
		insightsPage.StopCarouselAutoPlay();

		for (var i = 0; i < swipeCount; i++)
			insightsPage.SwipeCarouselRight();

		var carouselTitle = insightsPage.GetActiveCarouselArticleTitle();
		var articlePage = insightsPage.ClickReadMore();
		var articleTitle = articlePage.GetArticleTitle();

		articleTitle.Should().ContainEquivalentOf(carouselTitle);
	}
}
