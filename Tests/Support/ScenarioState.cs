using Core.Helpers;
using OpenQA.Selenium;
using PageObjects;

namespace Tests.Support;

public class ScenarioState
{
	public IWebDriver Driver { get; set; } = null!;
	public WaitHelper Wait { get; set; } = null!;
	public PageSetupHelper PageSetup { get; set; } = null!;
	public string BrowserKey { get; set; } = string.Empty;
	public HomePage? HomePage { get; set; }
	public ServicesCategoryPage? CategoryPage { get; set; }
}
