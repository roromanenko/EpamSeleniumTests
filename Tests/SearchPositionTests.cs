using FluentAssertions;
using OpenQA.Selenium;
using Tests.TestData;

namespace Tests;

public class SearchPositionTests : BaseTest
{
	private static class Locators
	{
		public static readonly By CareersLink = By.LinkText("Careers");
		public static readonly By StartSearchButton = By.XPath("//div[@class='pinned-button']//span[text()='Start Your Search Here']");
		public static readonly By KeywordsInput = By.Name("search");
		public static readonly By RemoteOption = By.XPath("//span[text()='Remote']");
		public static readonly By SearchButton = By.XPath("//form[@aria-label='form']//descendant::button");
		public static readonly By LatestJobResult = By.XPath("(//a[@data-testid='job-card-link'])[last()]");
		public static readonly By ApplyButton = By.Id("cta_job_apply_unauthorized");
		public static readonly By ApplicationHeading = By.CssSelector("#modal-title");
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
	/// 9. Validate that "Application" heading is present on the page
	/// </summary>
	[TestCaseSource(typeof(SearchTestData), nameof(SearchTestData.SearchPositionData))]
	public void ValidateUserCanSearchForPositionByCriteria(string keyword)
	{
		Driver!.Navigate().GoToUrl(Config.BaseUrl);
		Wait!.WaitForElementClickable(Locators.CareersLink).Click();
		Wait.JsClick(Locators.StartSearchButton);
		Wait.HandleCookieBanner();

		var keywordsInput = Wait.WaitForElement(Locators.KeywordsInput);
		keywordsInput.Clear();
		keywordsInput.SendKeys(keyword);

		Wait.WaitForElementClickable(Locators.RemoteOption).Click();
		Wait.JsClick(Locators.SearchButton);
		Wait.WaitForElementClickable(Locators.LatestJobResult).Click();
		Wait.WaitForElementClickable(Locators.ApplyButton).Click();

		var applicationHeading = Wait.WaitForElement(Locators.ApplicationHeading);
		applicationHeading.Should().NotBeNull();
		applicationHeading.Displayed.Should().BeTrue();
	}
}
