using NLog;

namespace Core.Helpers;

/// <summary>
/// Provides file-system helpers for download verification in tests.
/// </summary>
public static class FileDownloadHelper
{
	private static readonly Logger _log = LogManager.GetCurrentClassLogger();

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
		ArgumentException.ThrowIfNullOrEmpty(directoryPath);
		ArgumentException.ThrowIfNullOrEmpty(fileName);

		var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);
		var targetPath = Path.Combine(directoryPath, fileName);
		var partialPath = targetPath + ".crdownload";

		while (DateTime.UtcNow < deadline)
		{
			if (File.Exists(targetPath) && !File.Exists(partialPath))
			{
				_log.Info("File downloaded: {FileName}", fileName);
				return true;
			}

			Thread.Sleep(PollIntervalMs);
		}

		_log.Warn("File download timed out after {Timeout}s: {FileName}", timeoutSeconds, fileName);
		return false;
	}

	/// <summary>
	/// Deletes all files in the given directory without removing the directory itself.
	/// </summary>
	/// <param name="directoryPath">Directory to clean.</param>
	public static void CleanDownloadDirectory(string directoryPath)
	{
		ArgumentException.ThrowIfNullOrEmpty(directoryPath);

		if (!Directory.Exists(directoryPath))
			return;

		foreach (var file in Directory.GetFiles(directoryPath))
			File.Delete(file);
	}
}
