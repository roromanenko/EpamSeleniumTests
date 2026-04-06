using Core.Helpers;
using OpenQA.Selenium;

namespace PageObjects;

public class CareersPage : BasePage
{
	#region Locators

	private static readonly By _startSearchButton = By.XPath("//div[@class='pinned-button']//span[text()='Start Your Search Here']");

	private static readonly By _keywordsInput = By.ClassName("SearchBox_input__sJnt2");

	private static readonly By _remoteOption = By.XPath("//span[text()='Remote'][1]");

	private static readonly By _searchButton = By.XPath("//span[contains(text(), 'SEARCH')]");

	private static readonly By _latestJobResult =
		By.XPath("(//div[@class='JobCard_panel__gTD7e'])[last()]/div[@role='group']/div/div[@class='AccordionSection_title__L0ERa JobCard_accordionTitle__D1KeP']//a");

	#endregion

	public CareersPage(IWebDriver driver, WaitHelper wait) : base(driver, wait) { }

	#region Actions

	/// <summary>
	/// Performs a job search by keyword, selects the Remote option, and opens the latest result.
	/// </summary>
	/// <param name="keyword">The search keyword to enter into the job search input.</param>
	/// <returns>A <see cref="JobApplicationPage"/> for the opened job listing.</returns>
	public JobApplicationPage SearchForPosition(string keyword)
	{
		Click(_startSearchButton);
		Wait.HandleCookieBanner();

		Type(_keywordsInput, keyword);

		Click(_remoteOption);
		Click(_searchButton);
		Click(_latestJobResult);

		return new JobApplicationPage(Driver, Wait);
	}

	#endregion
}
