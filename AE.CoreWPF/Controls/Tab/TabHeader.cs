using System.Windows;
using System.Windows.Controls;

using ModernWpf.Controls;

namespace AE.CoreWPF.Controls;

public class TabHeader : Border
{
	private readonly CloseTabAction onClose;

	public string Title
	{
		get => textBlock.Text;
		set => textBlock.Text = value;
	}

	protected Border topBorder;
	protected TextBlock textBlock;

	public TabHeader(string title, CloseTabAction close)
	{
		onClose = close;

		Layout();
		SetSelect(false);

		Title = DisplayHelper.GetText(title);
	}

	protected virtual void Layout()
	{
		BorderBrush = DisplayHelper.Settings.SecondaryBackgroundBrush;
		BorderThickness = new Thickness(DisplayHelper.Settings.Border, 0, DisplayHelper.Settings.Border, 0);

		var panel = new SimpleStackPanel
		{
			Background = DisplayHelper.Settings.TransperentBrush,
			Orientation = Orientation.Horizontal,
			Spacing = DisplayHelper.Settings.MaxSpace,
		};

		textBlock = DisplayHelper.CreateTextBlock(fontSize: DisplayHelper.Settings.FontSizeCompact)
			.SetTextPadding(DisplayHelper.Settings.MaxSpace, 0, onClose != null ? 0 : DisplayHelper.Settings.MaxSpace, 0);

		panel.Children.Add(textBlock);

		if (onClose != null)
		{
			var closeBtn = DisplayHelper.CreateButton(Symbol.Cancel, DisplayHelper.Settings.ControlSizeCompact)
				.SetPadding(0, DisplayHelper.Settings.MinBorder, 0, 0)
				.SetOpacity(0)
				.SetCornerRadius(0)
				.SetButtonBackgroundPointerOver(DisplayHelper.Settings.StrokeBrush)
				.SetButtonBackgroundPressed(DisplayHelper.Settings.Stroke.WithAlpha(DisplayHelper.Settings.ColorOpacity2).ToBrush());

			closeBtn.Click += (s, e) => Close();

			panel.Children.Add(closeBtn);

			MouseEnter += (s, e) => closeBtn.Opacity = 1;
			MouseLeave += (s, e) => closeBtn.Opacity = 0;
		}

		topBorder = new Border
		{
			Height = DisplayHelper.Settings.Border,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Top,
			BorderThickness = new Thickness(0),
		};

		var grid = new Grid();

		grid.Children.Add(panel);
		grid.Children.Add(topBorder);

		Child = grid;

		if (onClose != null)
		{
			ContextMenu = DisplayHelper.CreateContextMenu();

			ContextMenu.Items.Add(DisplayHelper.CreateSubMenuItem(Symbol.Cancel, "&=close", Close));
			ContextMenu.Items.Add(new Separator());
			ContextMenu.Items.Add(DisplayHelper.CreateSubMenuItem(null, "&=close_all", CloseAll));
			ContextMenu.Items.Add(DisplayHelper.CreateSubMenuItem(null, "&=close_all_but_this", CloseAllButThis));
		}
	}

	public virtual void SetSelect(bool select)
	{
		topBorder.Height = select
			? DisplayHelper.Settings.Border
			: DisplayHelper.Settings.MinBorder;

		topBorder.Background = select
			? DisplayHelper.Settings.AccentBrush
			: DisplayHelper.Settings.SecondaryBackgroundBrush;
	}

	private void Close()
	{
		onClose(Title, false, false);
	}

	private void CloseAll()
	{
		onClose(Title, true, false);
	}

	private void CloseAllButThis()
	{
		onClose(Title, true, true);
	}
}

public class VariantTabHeader : TabHeader
{
	public VariantTabHeader(string title, CloseTabAction close) : base(title, close) { }

	protected override void Layout()
	{
		base.Layout();

		BorderThickness = new Thickness(DisplayHelper.Settings.MinBorder, 0, DisplayHelper.Settings.MinBorder, DisplayHelper.Settings.MinBorder);

		textBlock
			.SetTextPadding(textBlock.Padding.Left, DisplayHelper.Settings.MinSpace, textBlock.Padding.Right, DisplayHelper.Settings.MinSpace);

		topBorder.Background = DisplayHelper.Settings.StrokeBrush;
		topBorder.Height = DisplayHelper.Settings.MinBorder;
		topBorder.Margin = new Thickness(-DisplayHelper.Settings.MinBorder, 0, -DisplayHelper.Settings.MinBorder, 0);
	}

	public override void SetSelect(bool select)
	{
		topBorder.Visibility = select
			? Visibility.Collapsed
			: Visibility.Visible;

		BorderBrush = select
			? DisplayHelper.Settings.StrokeBrush
			: DisplayHelper.Settings.SecondaryBackgroundBrush;
	}
}