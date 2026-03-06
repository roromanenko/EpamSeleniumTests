namespace Tests.TestData;

/// <summary>
/// Provides test data for search position test cases.
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
}
