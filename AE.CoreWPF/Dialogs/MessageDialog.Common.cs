using System.Windows;
using System.Windows.Controls;

using ModernWpf.Controls;

namespace AE.CoreWPF.Dialogs;

public partial class MessageDialog : DialogWindow
{
	private MessageDialog(string message, string ok, string cancel, string close, bool showIcon)
	{
		Layout(message, ok, cancel, close, showIcon);
	}

	private void Layout(string message, string ok, string cancel, string close, bool showIcon)
	{
		Width = 300;
		Height = 125;

		Background = DisplayHelper.Settings.TertiaryBackgroundBrush;
		BorderBrush = DisplayHelper.Settings.SecondaryBackgroundBrush;
		ResizeMode = ResizeMode.NoResize;

		var grid = new Grid();

		grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
		grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(DisplayHelper.Settings.ControlSizeCompact2 + DisplayHelper.Settings.MaxSpace, GridUnitType.Pixel) });

		var scroll = DisplayHelper.CreateScroll(ScrollBarVisibility.Disabled);

		var textBlock = DisplayHelper.CreateTextBlock(message)
			.SetTextPadding(DisplayHelper.Settings.MaxSpace, DisplayHelper.Settings.MinSpace, DisplayHelper.Settings.MaxSpace, DisplayHelper.Settings.MaxSpace);

		textBlock.MaxWidth = Width;
		textBlock.TextWrapping = TextWrapping.Wrap;

		scroll.Content = textBlock;

		var bottomBorder = new Border
		{
			BorderBrush = DisplayHelper.Settings.StrokeBrush,
			Background = DisplayHelper.Settings.TertiaryBackgroundBrush,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			Margin = new Thickness(0),
			Padding = new Thickness(0),
			BorderThickness = new Thickness(0),
		};

		var actionGrid = new Grid
		{
			Margin = new Thickness(DisplayHelper.Settings.Space, 0, DisplayHelper.Settings.Space, 0),
		};

		actionGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
		actionGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
		actionGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

		var actionPanel1 = new SimpleStackPanel
		{
			Spacing = DisplayHelper.Settings.Space,
			Orientation = Orientation.Horizontal,
		};

		var actionPanel2 = new SimpleStackPanel
		{
			Spacing = DisplayHelper.Settings.Space,
			Orientation = Orientation.Horizontal,
		};

		Grid.SetColumn(actionPanel2, 2);

		if (ok != null)
		{
			var okBtn = DisplayHelper.CreateTextIconButtonVariant(showIcon ? Symbol.Accept : null, ok, DisplayHelper.Settings.ControlSizeCompact2, color: DisplayHelper.Settings.SuccessColor);

			okBtn.Click += (s, e) =>
			{
				DialogResult = true;
				Close();
			};

			if (close != null)
				actionPanel1.Children.Add(okBtn);
			else
				actionPanel2.Children.Add(okBtn);
		}

		if (cancel != null)
		{
			var cancelBtn = DisplayHelper.CreateTextIconButtonVariant(showIcon ? Symbol.Cancel : null, cancel, DisplayHelper.Settings.ControlSizeCompact2, color: DisplayHelper.Settings.ErrorColor);

			cancelBtn.Click += (s, e) =>
			{
				DialogResult = false;
				Close();
			};

			if (close != null)
				actionPanel1.Children.Add(cancelBtn);
			else
				actionPanel2.Children.Add(cancelBtn);
		}

		if (close != null)
		{
			var closeBtn = DisplayHelper.CreateTextIconButtonVariant(text: close, size: DisplayHelper.Settings.ControlSizeCompact2);

			closeBtn.Click += (s, e) => Close();

			actionPanel2.Children.Add(closeBtn);
		}

		actionGrid.Children.Add(actionPanel1);
		actionGrid.Children.Add(actionPanel2);

		Grid.SetRow(bottomBorder, 1);
		Grid.SetRow(actionGrid, 1);

		grid.Children.Add(scroll);
		grid.Children.Add(bottomBorder);
		grid.Children.Add(actionGrid);

		Content = grid;
	}
}
