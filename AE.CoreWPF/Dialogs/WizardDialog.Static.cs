using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using AE.CoreWPF.Dialogs.Wizard;

namespace AE.CoreWPF.Dialogs;

public class WizardDialogBuilder
{
	public static WizardDialogBuilder Create(string finish = "&=dialog_finish", string cancel = "&=dialog_cancel")
	{
		return new WizardDialogBuilder(finish, cancel);
	}

	private WizardDialog Dialog { get; }

	private WizardDialogBuilder(string finish, string cancel)
	{
		Dialog = new WizardDialog(finish, cancel)
		{
			Owner = Application.Current.MainWindow,
		};
	}

	public WizardDialogBuilder Size(double width, double height)
	{
		Dialog.Width = width;
		Dialog.Height = height;

		return this;
	}

	public WizardDialogBuilder MinSize(double width, double height)
	{
		Dialog.MinWidth = width;
		Dialog.MinHeight = height;

		return this;
	}

	public WizardDialogBuilder MaxSize(double width, double height)
	{
		Dialog.MaxWidth = width;
		Dialog.MaxHeight = height;

		return this;
	}

	public WizardDialogBuilder AddStep(IWizardStep step)
	{
		Dialog.Steps.Add(step);
		return this;
	}

	public IDictionary<string, object> Show()
	{
		if (Dialog.Steps.Count == 0)
			throw new ArgumentNullException(nameof(Dialog.Steps));

		Dialog.Next();

		if (Dialog.ShowDialog() == true)
			return Dialog.Steps.Last().Result();

		return null;
	}
}