using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ModernWpf.Controls.Primitives;

namespace AE.CoreWPF.Dialogs;

public class DialogWindow : Window
{
    public DialogWindow()
    {
        WindowHelper.SetUseModernWindowStyle(this, true);

        Resources.Add("SystemControlForegroundBaseHighBrush", DisplayHelper.Settings.SecondaryForegroundBrush);

        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        WindowStyle = WindowStyle.ToolWindow;
    }
}
