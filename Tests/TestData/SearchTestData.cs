namespace Tests.TestData;

/// <summary>
/// Provides test data for test cases.
/// </summary>
public static class SearchTestData
{
	public static IEnumerable<TestCaseData> SearchPositionData =>
	[
		new TestCaseData(".NET", "Ukraine"),
		new TestCaseData("Python", "Ukraine"),
		new TestCaseData("DevOps", "Ukraine"),
		new TestCaseData("Java", "Ukraine"),
	];

	public static IEnumerable<TestCaseData> GlobalSearchData =>
	[
		new TestCaseData("Blockchain"),
		new TestCaseData("Cloud"),
		new TestCaseData("Automation"),
	];
}
