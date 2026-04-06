using Core.Helpers;
using OpenQA.Selenium;

namespace PageObjects;

public class HomePage : BasePage
{
	#region Locators

	private static readonly By _magnifierIcon = By.XPath("//span[@class='search-icon dark-icon header-search__search-icon']");

	private static readonly By _careersLink = By.LinkText("Careers");

	#endregion

	public HomePage(IWebDriver driver, WaitHelper wait) : base(driver, wait) { }

	#region Actions

	/// <summary>
	/// Clicks the magnifier icon to open the global search overlay.
	/// </summary>
	/// <returns>A <see cref="GlobalSearchPage"/> representing the search overlay.</returns>
	public GlobalSearchPage OpenSearch()
	{
		Click(_magnifierIcon);
		return new GlobalSearchPage(Driver, Wait);
	}

	/// <summary>
	/// Clicks the Careers navigation link and returns the resulting page.
	/// </summary>
	/// <returns>A <see cref="CareersPage"/> representing the Careers page.</returns>
	public CareersPage GoToCareers()
	{
		Click(_careersLink);
		return new CareersPage(Driver, Wait);
	}

	#endregion
}
