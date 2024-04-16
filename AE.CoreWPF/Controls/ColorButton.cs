using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;

namespace AE.CoreWPF.Controls;

public class ColorButton : DropDownButton
{
	public Color SelectColor
	{
		get => colorPicker.SelectColor;
		set => colorPicker.SelectColor = value;
	}

	private ColorPicker colorPicker;

	public ColorButton(double size, double scale)
	{
		Layout(size, scale);
	}

	public void Layout(double size, double scale)
	{
		Background = DisplayHelper.Settings.TertiaryBackgroundBrush;

		ControlHelper.SetCornerRadius(this, new CornerRadius(DisplayHelper.Settings.MinRound));

		colorPicker = new ColorPicker
		{
			Background = DisplayHelper.Settings.TertiaryBackgroundBrush,
			ColorOpacityEnabled = false,
			SnapsToDevicePixels = true,
		};

		var viewBox = new Border
		{
			Width = size * (1 / scale),
			Height = size * (1 / scale),
			CornerRadius = new CornerRadius(DisplayHelper.Settings.Round),
			LayoutTransform = new ScaleTransform(scale, scale),
		};

		colorPicker.SelectColorChanged += (s, e) =>
		{
			viewBox.Background = colorPicker.SelectColor.ToBrush();
		};

		Content = viewBox;
		Flyout = new Flyout
		{
			Placement = FlyoutPlacementMode.BottomEdgeAlignedLeft,
			Content = colorPicker,
		};
	}
}
