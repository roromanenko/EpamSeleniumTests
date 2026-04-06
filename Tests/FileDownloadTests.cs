using Core.Helpers;
using FluentAssertions;
using PageObjects;
using Tests.TestData;

namespace Tests;

[TestFixture]
public class FileDownloadTests : BaseTest
{
	protected override string BrowserKey => "chrome-download";

	private string _downloadDir => Path.GetFullPath(Path.Combine(Path.GetTempPath(), Config.DownloadDirectory));

	[SetUp]
	public void SetUpDownloadDirectory()
	{
		Directory.CreateDirectory(_downloadDir);
		FileDownloadHelper.CleanDownloadDirectory(_downloadDir);
	}

	[TearDown]
	public void TearDownDownloadDirectory()
	{
		FileDownloadHelper.CleanDownloadDirectory(_downloadDir);
	}

	/// <summary>
	/// TC-3: Validate file download function works as expected<br/>
	/// 1. Navigate to https://www.epam.com/<br/>
	/// 2. Scroll down to the page footer<br/>
	/// 3. Click on "Code of Ethical Conduct (PDF)" in the Policies section<br/>
	/// 4. Wait till file is downloaded<br/>
	/// 5. Validate that the expected file was downloaded
	/// </summary>
	[TestCaseSource(typeof(FileDownloadTestData), nameof(FileDownloadTestData.FileDownloadData))]
	public void ValidateFileDownloadWorksAsExpected(string expectedFileName)
	{
		var homePage = new HomePage(Driver!, Wait!, PageSetup);
		homePage.NavigateTo(Config.BaseUrl);

		PageSetup.HandleCookieBanner();

		homePage.DownloadPolicyPdf();

		FileDownloadHelper.WaitForFileDownload(_downloadDir, expectedFileName, Config.Timeouts.ExplicitWait);

		File.Exists(Path.Combine(_downloadDir, expectedFileName))
			.Should().BeTrue($"Expected file '{expectedFileName}' to be downloaded to '{_downloadDir}'");
	}
}
