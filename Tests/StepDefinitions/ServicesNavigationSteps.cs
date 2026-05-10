using Core.Configuration;
using Core.Interfaces;
using FluentAssertions;
using PageObjects;
using Reqnroll;
using Tests.Support;

namespace Tests.StepDefinitions;

[Binding]
public class ServicesNavigationSteps
{
	private readonly ScenarioState _state;
	private readonly TestConfiguration _config;

	public ServicesNavigationSteps(ScenarioState state, ITestConfigurationProvider configProvider)
	{
		_state = state;
		_config = configProvider.GetConfiguration();
	}

	[Given(@"I am on the EPAM home page")]
	public void GivenIAmOnTheEpamHomePage()
	{
		_state.HomePage = new HomePage(_state.Driver, _state.Wait, _state.PageSetup);
		_state.HomePage.NavigateTo(_config.BaseUrl);
		_state.PageSetup.HandleCookieBanner();
	}

	[When(@"I open the Services menu and select ""(.*)""")]
	public void WhenIOpenTheServicesMenuAndSelect(string category)
	{
		var servicesMenu = _state.HomePage!.OpenServicesMenu();
		_state.CategoryPage = servicesMenu.SelectCategory(category);
	}

	[Then(@"the page heading should be ""(.*)""")]
	public void ThenThePageHeadingShouldBe(string expectedHeading)
	{
		_state.CategoryPage!.IsHeadingDisplayed(expectedHeading)
			.Should().BeTrue($"page heading '{expectedHeading}' should be displayed");
	}

	[Then(@"the ""Our Related Expertise"" section should be displayed")]
	public void ThenTheOurRelatedExpertiseSectionShouldBeDisplayed()
	{
		_state.CategoryPage!.IsRelatedExpertiseDisplayed()
			.Should().BeTrue("'Our Related Expertise' section should be displayed");
	}
}
