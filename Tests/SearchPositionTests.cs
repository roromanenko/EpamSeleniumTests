using FluentAssertions;
using PageObjects;
using Tests.TestData;

namespace Tests;

public class SearchPositionTests : BaseTest
{
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
	/// 9. Validate that the programming language mentioned above is present on the page
	/// </summary>
	[TestCaseSource(typeof(SearchTestData), nameof(SearchTestData.SearchPositionData))]
	public void ValidateUserCanSearchForPositionByCriteria(string keyword, string location)
	{
		var homePage = new HomePage(Driver!, Wait!);
		homePage.NavigateTo(Config.BaseUrl);

		var careersPage = homePage.GoToCareers();
		var jobApplicationPage = careersPage.SearchForPosition(keyword);

		jobApplicationPage.OpenApplication();
		var applicationHeading = jobApplicationPage.GetApplicationHeading();

		applicationHeading.Should().NotBeNull();
		applicationHeading.Displayed.Should().BeTrue();
	}
}
