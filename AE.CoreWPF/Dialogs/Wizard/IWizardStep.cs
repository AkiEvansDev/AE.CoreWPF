using System.Collections.Generic;
using System.Windows;

namespace AE.CoreWPF.Dialogs.Wizard;

public interface IWizardStep
{
	string Title { get; }
	string Prev { get; }
	string Next { get; }

	UIElement Layout();

	void Init(IDictionary<string, object> parameters);
	IDictionary<string, object> Result();

	void BeforeOpen();
	bool BeforeClose();
}
