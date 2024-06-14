using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using AE.CoreWPF.Controls;
using AE.CoreWPF.Dialogs.Wizard;

using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;

namespace AE.CoreWPF.Dialogs;

internal class WizardDialog : DialogWindow
{
	private int currentStep = -1;
	public ICollection<IWizardStep> Steps { get; }

	private Grid grid;
	private TextBlock stepTitle;
	private ScrollViewerEx scroll;
	private TextIconButton prev;
	private TextIconButton next;
	private TextIconButton finish;
	private TextIconButton cancel;

	public WizardDialog(string finish, string cancel)
	{
		Steps = new List<IWizardStep>();

		Layout(finish, cancel);
	}

	private void Layout(string finishText, string cancelText)
	{
		TitleBar.SetExtendViewIntoTitleBar(this, true);

		Background = DisplayHelper.Settings.SecondaryBackgroundBrush;
		BorderBrush = DisplayHelper.Settings.SecondaryBackgroundBrush;
		Width = 800;
		Height = 600;
		ResizeMode = ResizeMode.NoResize;

		grid = new Grid();

		grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(TitleBar.GetHeight(this), GridUnitType.Pixel) });
		grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
		grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(DisplayHelper.Settings.ControlSize + DisplayHelper.Settings.MaxSpace, GridUnitType.Pixel) });

		var topBorder = new Border
		{
			BorderBrush = DisplayHelper.Settings.StrokeBrush,
			Background = DisplayHelper.Settings.TertiaryBackgroundBrush,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			Margin = new Thickness(0),
			Padding = new Thickness(0),
			BorderThickness = new Thickness(0),
		};

		stepTitle = DisplayHelper.CreateTextBlock(fontWeight: FontWeights.Bold)
			.SetMargin(DisplayHelper.Settings.MaxSpace, 0);

		scroll = DisplayHelper.CreateScroll(ScrollBarVisibility.Auto)
			.SetPadding(0, DisplayHelper.Settings.Space);

		var actionPanel = new SimpleStackPanel
		{
			Spacing = DisplayHelper.Settings.MaxSpace,
			Orientation = Orientation.Horizontal,
			HorizontalAlignment = HorizontalAlignment.Right,
			Margin = new Thickness(DisplayHelper.Settings.MaxSpace, 0, DisplayHelper.Settings.MaxSpace, 0),
		};

		prev = DisplayHelper.CreateTextIconButtonVariant(size: DisplayHelper.Settings.ControlSize);
		next = DisplayHelper.CreateTextIconButtonVariant(size: DisplayHelper.Settings.ControlSize);
		finish = DisplayHelper.CreateTextIconButtonVariant(text: finishText, size: DisplayHelper.Settings.ControlSize);
		cancel = DisplayHelper.CreateTextIconButtonVariant(text: cancelText, size: DisplayHelper.Settings.ControlSize);

		prev.Click += (s, e) => Prev();
		next.Click += (s, e) => Next();
		finish.Click += (s, e) =>
		{
			DialogResult = true;
			Close();
		};
		cancel.Click += (s, e) =>
		{
			DialogResult = false;
			Close();
		};

		actionPanel.Children.Add(prev);
		actionPanel.Children.Add(next);
		actionPanel.Children.Add(finish);
		actionPanel.Children.Add(cancel);

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

		Grid.SetRow(scroll, 1);
		Grid.SetRow(bottomBorder, 2);
		Grid.SetRow(actionPanel, 2);

		grid.Children.Add(topBorder);
		grid.Children.Add(stepTitle);
		grid.Children.Add(scroll);
		grid.Children.Add(bottomBorder);
		grid.Children.Add(actionPanel);

		Content = grid;

		MouseDown += (s, e) =>
		{
			if (e.LeftButton == MouseButtonState.Pressed)
				Keyboard.ClearFocus();
		};
	}

	public void Prev()
	{
		currentStep--;
		SetStep(currentStep);
	}

	public void Next()
	{
		currentStep++;
		SetStep(currentStep);
	}

	private void SetStep(int index)
	{
		if (index < 0 || index >= Steps.Count)
			return;

		var step = Steps.ElementAt(index);

		stepTitle.Text = DisplayHelper.GetText(step.Title);
		prev.Text = DisplayHelper.GetText(step.Prev);
		next.Text = DisplayHelper.GetText(step.Next);

		if (index == 0)
		{
			prev.Visibility = Visibility.Collapsed;
			step.Init(null);
		}
		else if (index > 0)
		{
			prev.Visibility = Visibility.Visible;
			step.Init(Steps.ElementAt(index - 1).Result());
		}

		if (index == Steps.Count - 1)
		{
			next.Visibility = Visibility.Collapsed;
			finish.Visibility = Visibility.Visible;
		}
		else
		{
			next.Visibility = Visibility.Visible;
			finish.Visibility = Visibility.Collapsed;
		}

		step.BeforeOpen();
		scroll.Content = step.Layout();
	}
}
