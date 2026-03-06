using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.TestData;

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
