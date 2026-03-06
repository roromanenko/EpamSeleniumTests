namespace Tests.TestData;

/// <summary>
/// Provides test data for test cases.
/// </summary>
public static class SearchTestData
{
	public static IEnumerable<TestCaseData> SearchPositionData =>
	[
		new TestCaseData(".NET"),
		new TestCaseData("Python"),
		new TestCaseData("DevOps"),
		new TestCaseData("Java"),
	];

	public static IEnumerable<TestCaseData> GlobalSearchData =>
	[
		new TestCaseData("Blockchain"),
		new TestCaseData("Cloud"),
		new TestCaseData("Automation"),
	];
}
