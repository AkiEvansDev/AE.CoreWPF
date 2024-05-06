using System.Windows;

namespace AE.CoreWPF.Dialogs;

public partial class MessageDialog
{
	public static bool? ShowCloseMessage(string message, string title = "&=message_dialog_title", string close = "&=dialog_close", bool showIcon = true)
	{
		return ShowMessage(message, title, null, null, close, showIcon);
	}

	public static bool? ShowOkMessage(string message, string title = "&=message_dialog_title", string ok = "&=dialog_ok", bool showIcon = true)
	{
		return ShowMessage(message, title, ok, null, null, showIcon);
	}

	public static bool? ShowOkCancelMessage(string message, string title = "&=message_dialog_title", string ok = "&=dialog_ok", string cancel = "&=dialog_cancel", bool showIcon = true)
	{
		return ShowMessage(message, title, ok, cancel, null, showIcon);
	}

	public static bool? ShowMessage(string message, string title = "&=message_dialog_title", string ok = "&=dialog_ok", string cancel = "&=dialog_cancel", string close = "&=dialog_close", bool showIcon = true)
	{
		var dialog = new MessageDialog(message, ok, cancel, close, showIcon)
		{
			Owner = Application.Current.MainWindow,
			Title = string.IsNullOrEmpty(title) ? "" : DisplayHelper.GetText(title),
		};

		return dialog.ShowDialog();
	}
}
