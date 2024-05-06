using System.Windows;

namespace AE.CoreWPF.Dialogs;

public partial class FileDialog
{
	internal static string[] KnownFolders { get; }
	internal static string[] ImageExtensions { get; } = new[]
	{
		".png",
		".jpg",
		".jpeg",
	};

	static FileDialog()
	{
		KnownFolders = new string[]
		{
			WinFileHelper.GetKnownFolderPath(WinFileHelper.Desktop),
			WinFileHelper.GetKnownFolderPath(WinFileHelper.Downloads),
			WinFileHelper.GetKnownFolderPath(WinFileHelper.Documents),
			WinFileHelper.GetKnownFolderPath(WinFileHelper.Pictures),
			WinFileHelper.GetKnownFolderPath(WinFileHelper.Videos),
		};
	}

	public static string SelectPath(string basePath = null, string title = "&=file_dialog_title", params string[] extensionFilters)
	{
		return Select(basePath, title, true, true, extensionFilters);
	}

	public static string SelectFolder(string basePath = null, string title = "&=file_dialog_title", params string[] extensionFilters)
	{
		return Select(basePath, title, true, false, extensionFilters);
	}

	public static string SelectFile(string basePath = null, string title = "&=file_dialog_title", params string[] extensionFilters)
	{
		return Select(basePath, title, false, true, extensionFilters);
	}

	private static string Select(string basePath, string title, bool canSelectFolder, bool canSelectFile, string[] extensionFilters)
	{
		var dialog = new FileDialog(basePath, canSelectFolder, canSelectFile, extensionFilters)
		{
			Owner = Application.Current.MainWindow,
			Title = string.IsNullOrEmpty(title) ? "" : DisplayHelper.GetText(title),
		};

		dialog.ShowDialog();

		return dialog.result;
	}
}
