using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;

namespace AE.CoreWPF.Controls;

public class ColorButton : DropDownButton
{
	private ColorPicker colorPicker;

	public Color SelectColor
	{
		get => colorPicker.SelectColor;
		set => colorPicker.SelectColor = value;
	}

	public ColorButton(double size, double scale)
	{
		Layout(size, scale);
	}

	public void Layout(double size, double scale)
	{
		//Style = (Style)Application.Current.Resources["DefaultButtonStyle"];
		colorPicker = new ColorPicker
		{
			Background = DisplayHelper.Settings.TertiaryBackgroundBrush,
			ColorOpacityEnabled = false,
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
