using FluentAssertions;
using OpenQA.Selenium;
using Tests.TestData;

namespace Tests;

public class SearchPositionTests : BaseTest
{
	[TestCaseSource(typeof(SearchTestData), nameof(SearchTestData.SearchPositionData))]
	public void ValidateUserCanSearchForPositionByCriteria(string keyword)
	{
		Driver!.Navigate().GoToUrl(Config.BaseUrl);

		Wait!.WaitForElementClickable(By.LinkText("Careers")).Click();

		Wait.JsClick(By.XPath("//div[@class='pinned-button']//span[text()='Start Your Search Here']"));

		Wait!.AcceptCookiesIfPresent();
		Wait.WaitForCookieBannerToDisappear();

		var keywordsInput = Wait.WaitForElement(By.ClassName("SearchBox_input__sJnt2"));
		keywordsInput.Clear();
		keywordsInput.SendKeys(keyword);

		Wait.WaitForElementClickable(By.XPath("//span[text()='Remote'][1]")).Click();

		Wait.JsClick(By.XPath("//span[contains(text(), 'SEARCH')]"));

		Wait.WaitForElementClickable(By.XPath("(//div[@class='JobCard_panel__gTD7e'])[last()]/div[@role='group']/div/div[@class='AccordionSection_title__L0ERa JobCard_accordionTitle__D1KeP']//a")).Click();

		Wait.WaitForElementClickable(By.Name("button_cta_job_apply_unauthorized")).Click();

		var applicationHeading = Wait.WaitForElement(By.XPath("//h2[text() = 'Application']"));
		applicationHeading.Should().NotBeNull();
		applicationHeading.Displayed.Should().BeTrue();
	}
}
