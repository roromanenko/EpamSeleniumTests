using Core.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace PageObjects;

public class HomePage : BasePage
{
	#region Locators

	private static readonly By _magnifierIcon = By.XPath("//span[@class='search-icon dark-icon header-search__search-icon']");

	private static readonly By _careersLink = By.LinkText("Careers");

	private static readonly By _insightsLink = By.LinkText("Insights");

	private static readonly By _policiesPdfLink = By.XPath("//footer//a[contains(text(),'Code of Ethical Conduct')]");

	private static readonly By _servicesNavLink = By.LinkText("Services");

	#endregion

	public HomePage(IWebDriver driver, WaitHelper wait, PageSetupHelper pageSetup) : base(driver, wait, pageSetup) { }

	#region Actions

	/// <summary>
	/// Clicks the magnifier icon to open the global search overlay.
	/// </summary>
	/// <returns>A <see cref="GlobalSearchPage"/> representing the search overlay.</returns>
	public GlobalSearchPage OpenSearch()
	{
		Click(_magnifierIcon);
		return new GlobalSearchPage(Driver, Wait, PageSetup);
	}

	/// <summary>
	/// Clicks the Careers navigation link and returns the resulting page.
	/// </summary>
	/// <returns>A <see cref="CareersPage"/> representing the Careers page.</returns>
	public CareersPage GoToCareers()
	{
		Click(_careersLink);
		return new CareersPage(Driver, Wait, PageSetup);
	}

	/// <summary>
	/// Clicks the Insights navigation link and returns the resulting page.
	/// </summary>
	/// <returns>An <see cref="InsightsPage"/> representing the Insights page.</returns>
	public InsightsPage GoToInsights()
	{
		Click(_insightsLink);
		return new InsightsPage(Driver, Wait, PageSetup);
	}

	/// <summary>
	/// Hovers over the Services navigation link to open the Services dropdown menu.
	/// </summary>
	/// <returns>A <see cref="ServicesMenu"/> representing the Services dropdown.</returns>
	public ServicesMenu OpenServicesMenu()
	{
		Hover(_servicesNavLink);
		return new ServicesMenu(Driver, Wait, PageSetup);
	}

	/// <summary>
	/// Scrolls to the "Code of Ethical Conduct (PDF)" link and clicks it via JavaScript.
	/// </summary>
	public void DownloadPolicyPdf()
	{
		Wait.ScrollIntoView(_policiesPdfLink);
		Wait.JsClick(_policiesPdfLink);
	}

	#endregion
}
