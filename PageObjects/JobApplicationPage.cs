using Core.Helpers;
using OpenQA.Selenium;

namespace PageObjects;

public class JobApplicationPage : BasePage
{
	#region Locators

	private static readonly By _applyButton = By.Name("button_cta_job_apply_unauthorized");

	private static readonly By _applicationHeading = By.XPath("//h2[text() = 'Application']");

	#endregion

	public JobApplicationPage(IWebDriver driver, WaitHelper wait, PageSetupHelper pageSetup) : base(driver, wait, pageSetup) { }

	#region Actions

	/// <summary>
	/// Clicks the Apply button to open the job application form.
	/// </summary>
	public void OpenApplication() => Click(_applyButton);

	/// <summary>
	/// Waits for and returns the Application section heading element.
	/// </summary>
	/// <returns>The <see cref="IWebElement"/> for the "Application" heading.</returns>
	public IWebElement GetApplicationHeading() => WaitForElement(_applicationHeading);

	#endregion
}
