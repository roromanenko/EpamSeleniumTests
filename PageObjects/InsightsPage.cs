using Core.Helpers;
using OpenQA.Selenium;

namespace PageObjects;

public class InsightsPage : BasePage
{
	#region Locators

	private static readonly By _carouselNextButton = By.CssSelector("button.slider__right-arrow");

	private static readonly By _activeCarouselTitle = By.CssSelector("div.owl-item.active p.scaling-of-text-wrapper");

	private static readonly By _readMoreButton = By.CssSelector("div.owl-item.active a.slider-cta-link");

	#endregion

	private const int CarouselScrollOffsetPx = 700;

	public InsightsPage(IWebDriver driver, WaitHelper wait, PageSetupHelper pageSetup) : base(driver, wait, pageSetup) { }

	#region Actions

	/// <summary>
	/// Stops the Owl Carousel auto-play via its jQuery event API.
	/// Call once after navigating to the Insights page to prevent the carousel from
	/// advancing slides on its own during the test.
	/// </summary>
	public void StopCarouselAutoPlay()
	{
		((IJavaScriptExecutor)Driver).ExecuteScript("$('.owl-carousel').trigger('stop.owl.autoplay');");
	}

	/// <summary>
	/// Scrolls the carousel next-arrow into view, then clicks it to advance one slide.
	/// The scroll is intentional — it validates scroll functionality as part of the test.
	/// Waits for the active slide title to be present after the click.
	/// </summary>
	public void SwipeCarouselRight()
	{
		ScrollByWheel(CarouselScrollOffsetPx);
		Click(_carouselNextButton);
		WaitForElement(_activeCarouselTitle);
	}

	/// <summary>
	/// Returns the text of the active carousel slide's title element.
	/// Selenium concatenates all nested span text automatically via .Text.
	/// </summary>
	public string GetActiveCarouselArticleTitle()
		=> Wait.WaitForNonEmptyText(_activeCarouselTitle);

	/// <summary>
	/// Clicks the "Read More" link on the active carousel slide.
	/// </summary>
	/// <returns>An <see cref="ArticlePage"/> for the opened article.</returns>
	public ArticlePage ClickReadMore()
	{
		Click(_readMoreButton);
		return new ArticlePage(Driver, Wait, PageSetup);
	}

	#endregion
}
