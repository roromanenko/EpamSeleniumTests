using Core.Helpers;
using OpenQA.Selenium;

namespace PageObjects;

public class ServicesCategoryPage : BasePage
{
	#region Locators

	private static By PageHeading(string headingText) => By.XPath(
		$"//div[@class='text']//span[contains(@class,'gradient-text') and normalize-space()='{headingText}']");

	private static readonly By _relatedExpertiseSection = By.XPath("//div[@class = 'text']//span[contains(text(), 'Our Related Expertise')]");

	#endregion

	public ServicesCategoryPage(IWebDriver driver, WaitHelper wait, PageSetupHelper pageSetup) : base(driver, wait, pageSetup) { }

	#region Actions

	/// <summary>
	/// Returns <see langword="true"/> if the page's main visual heading matches the expected text.
	/// </summary>
	public bool IsHeadingDisplayed(string expectedHeading) => IsDisplayed(PageHeading(expectedHeading));

	/// <summary>
	/// Returns <see langword="true"/> if the "Our Related Expertise" section is visible on the page.
	/// </summary>
	public bool IsRelatedExpertiseDisplayed() => IsDisplayed(_relatedExpertiseSection);

	#endregion
}
