using Core.Helpers;
using OpenQA.Selenium;

namespace PageObjects;

public class ServicesMenu : BasePage
{
	#region Locators

	private static By CategoryLink(string keyword) => By.XPath($"//a[@class='top-navigation__sub-link' and normalize-space(text())='{keyword}']");

	#endregion

	public ServicesMenu(IWebDriver driver, WaitHelper wait, PageSetupHelper pageSetup) : base(driver, wait, pageSetup) { }

	#region Actions

	/// <summary>
	/// Navigates to a Services sub-category by its visible link text.
	/// </summary>
	/// <param name="categoryLinkText">The visible link text of the service category (e.g. "Generative AI").</param>
	/// <returns>A <see cref="ServicesCategoryPage"/> representing the selected category landing page.</returns>
	public ServicesCategoryPage SelectCategory(string categoryLinkText)
	{
		Click(CategoryLink(categoryLinkText));
		return new ServicesCategoryPage(Driver, Wait, PageSetup);
	}

	#endregion
}
