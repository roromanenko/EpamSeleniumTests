namespace Core.Helpers;

/// <summary>
/// Provides file-system helpers for download verification in tests.
/// </summary>
public static class FileDownloadHelper
{
	private const int PollIntervalMs = 500;

	/// <summary>
	/// Polls the directory until the named file appears with no .crdownload partial suffix,
	/// or until the timeout elapses.
	/// </summary>
	/// <param name="directoryPath">Directory to watch.</param>
	/// <param name="fileName">Expected file name.</param>
	/// <param name="timeoutSeconds">Maximum seconds to wait.</param>
	/// <returns><c>true</c> if the file was found; <c>false</c> on timeout.</returns>
	public static bool WaitForFileDownload(string directoryPath, string fileName, int timeoutSeconds)
	{
		if (string.IsNullOrEmpty(directoryPath)) throw new ArgumentNullException(nameof(directoryPath));
		if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));

		var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);
		var targetPath = Path.Combine(directoryPath, fileName);
		var partialPath = targetPath + ".crdownload";

		while (DateTime.UtcNow < deadline)
		{
			if (File.Exists(targetPath) && !File.Exists(partialPath))
				return true;

			Thread.Sleep(PollIntervalMs);
		}

		return false;
	}

	/// <summary>
	/// Deletes all files in the given directory without removing the directory itself.
	/// </summary>
	/// <param name="directoryPath">Directory to clean.</param>
	public static void CleanDownloadDirectory(string directoryPath)
	{
		if (string.IsNullOrEmpty(directoryPath)) throw new ArgumentNullException(nameof(directoryPath));

		if (!Directory.Exists(directoryPath))
			return;

		foreach (var file in Directory.GetFiles(directoryPath))
			File.Delete(file);
	}
}
