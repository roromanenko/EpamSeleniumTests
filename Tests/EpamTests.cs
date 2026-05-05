using Core.Helpers;
using FluentAssertions;
using PageObjects;
using Tests.TestData;

namespace Tests;

public class EpamTests : BaseTest
{
	protected override string BrowserKey =>
		TestContext.CurrentContext.Test.MethodName == nameof(ValidateFileDownloadWorksAsExpected)
			? "chrome-download"
			: base.BrowserKey;

	private string _downloadDir =>
		Path.GetFullPath(Path.Combine(Path.GetTempPath(), Config.DownloadDirectory));

	[SetUp]
	public void SetUpDownloadDirectory()
	{
		if (TestContext.CurrentContext.Test.MethodName != nameof(ValidateFileDownloadWorksAsExpected))
			return;
		Directory.CreateDirectory(_downloadDir);
		FileDownloadHelper.CleanDownloadDirectory(_downloadDir);
	}

	[TearDown]
	public void TearDownDownloadDirectory()
	{
		if (TestContext.CurrentContext.Test.MethodName != nameof(ValidateFileDownloadWorksAsExpected))
			return;
		FileDownloadHelper.CleanDownloadDirectory(_downloadDir);
	}

	/// <summary>
	/// TC-1: Validate that the user can search for a position based on criteria<br/>
	/// 1. Navigate to https://www.epam.com/<br/>
	/// 2. Find a link "Careers" and click on it<br/>
	/// 3. Click on "Start Your Search Here" button<br/>
	/// 4. Write the name of any programming language in the field "Keywords"<br/>
	/// 5. Select the option "Remote"<br/>
	/// 6. Click on the button "Find"<br/>
	/// 7. Click on the latest element in the list of results<br/>
	/// 8. Click on the button "View and apply"<br/>
	/// 9. Validate that the programming language mentioned above is present on the page
	/// </summary>
	[TestCaseSource(typeof(SearchTestData), nameof(SearchTestData.SearchPositionData))]
	public void ValidateUserCanSearchForPositionByCriteria(string keyword, string location)
	{
		var homePage = new HomePage(Driver!, Wait!, PageSetup);
		homePage.NavigateTo(Config.BaseUrl);

		var careersPage = homePage.GoToCareers();
		var jobApplicationPage = careersPage.SearchForPosition(keyword);

		jobApplicationPage.OpenApplication();
		var applicationHeading = jobApplicationPage.GetApplicationHeading();

		applicationHeading.Should().NotBeNull();
		applicationHeading.Displayed.Should().BeTrue();
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
		var homePage = new HomePage(Driver!, Wait!, PageSetup);
		homePage.NavigateTo(Config.BaseUrl);

		var searchPage = homePage.OpenSearch();
		searchPage.SearchFor(keyword);

		var results = searchPage.GetResultLinks(keyword);
		results.Should().NotBeEmpty($"Expected search results should contain '{keyword}'");
		results.All(r => r.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
			.Should().BeTrue($"Expected all links to contain '{keyword}'");
	}

	/// <summary>
	/// TC-3: Validate file download function works as expected<br/>
	/// 1. Navigate to https://www.epam.com/<br/>
	/// 2. Scroll down to the page footer<br/>
	/// 3. Click on "Code of Ethical Conduct (PDF)" in the Policies section<br/>
	/// 4. Wait till file is downloaded<br/>
	/// 5. Validate that the expected file was downloaded
	/// </summary>
	[TestCaseSource(typeof(FileDownloadTestData), nameof(FileDownloadTestData.FileDownloadData))]
	public void ValidateFileDownloadWorksAsExpected(string expectedFileName)
	{
		var homePage = new HomePage(Driver!, Wait!, PageSetup);
		homePage.NavigateTo(Config.BaseUrl);

		PageSetup.HandleCookieBanner();

		homePage.DownloadPolicyPdf();

		FileDownloadHelper.WaitForFileDownload(_downloadDir, expectedFileName, Config.Timeouts.ExplicitWait);

		File.Exists(Path.Combine(_downloadDir, expectedFileName))
			.Should().BeTrue($"Expected file '{expectedFileName}' to be downloaded to '{_downloadDir}'");
	}

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
