using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using AE.CoreWPF.Controls;

using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;

namespace AE.CoreWPF;

public static partial class DisplayHelper
{
	public static TextBlock CreateTextBlock(string text = null, double? fontSize = null, FontWeight? fontWeight = null, double? opacity = null)
	{
		return CreateTextBlock(text, fontSize, fontWeight, Settings.ForegroundBrush, opacity);
	}

	public static TextBlock CreateSecondaryTextBlock(string text = null, double? fontSize = null, FontWeight? fontWeight = null, double? opacity = null)
	{
		return CreateTextBlock(text, fontSize, fontWeight, Settings.SecondaryForegroundBrush, opacity);
	}

	public static TextBlock CreateTertiaryTextBlock(string text = null, double? fontSize = null, FontWeight? fontWeight = null, double? opacity = null)
	{
		return CreateTextBlock(text, fontSize, fontWeight, Settings.TertiaryForegroundBrush, opacity);
	}

	private static TextBlock CreateTextBlock(string text, double? fontSize, FontWeight? fontWeight, Brush brush, double? opacity)
	{
		fontSize ??= Settings.FontSize;
		opacity ??= Settings.ControlOpacity;

		var element = new TextBlock
		{
			Foreground = brush,
			Text = GetText(text),
			TextAlignment = TextAlignment.Left,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			Padding = new Thickness(0),
			FontSize = fontSize.Value,
			FontWeight = fontWeight ?? FontWeights.Normal,
			Opacity = opacity.Value,
			SnapsToDevicePixels = true,
		};

		if (element.FontWeight == FontWeights.Bold)
			element.FontFamily = Settings.FontFamilyBold;
		else if (element.FontWeight == FontWeights.Medium)
			element.FontFamily = Settings.FontFamilyMedium;
		else if (element.FontWeight == FontWeights.Light)
			element.FontFamily = Settings.FontFamilyLight;

		return element;
	}

	public static CheckBox CreateCheckBox(double? size = null, double? scale = null)
	{
		size ??= Settings.ControlSize;
		scale ??= Settings.ControlScale;

		var element = new CheckBox
		{
			MinWidth = 0,
			MinHeight = 0,
			MaxWidth = double.PositiveInfinity,
			MaxHeight = double.PositiveInfinity,
			Width = size.Value,
			Height = size.Value,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			Padding = new Thickness(Settings.MinSpace),
			LayoutTransform = new ScaleTransform(scale.Value, scale.Value),
			FocusVisualStyle = null,
			SnapsToDevicePixels = true,
		};

		ControlHelper.SetCornerRadius(element, new CornerRadius(Settings.MinRound));

		return element;
	}

	public static TextBox CreateTextBox(double? size = null, double? scale = null, double? fontSize = null, string pattern = null)
	{
		size ??= Settings.ControlSize;
		scale ??= Settings.ControlScale;
		fontSize ??= Settings.FontSize;

		var element = new TextBox
		{
			Background = Settings.TransperentBrush,
			Foreground = Settings.ForegroundBrush,
			MinWidth = 0,
			MinHeight = 0,
			MaxWidth = double.PositiveInfinity,
			MaxHeight = double.PositiveInfinity,
			Height = size.Value,
			TextAlignment = TextAlignment.Left,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalContentAlignment = HorizontalAlignment.Left,
			VerticalContentAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			Padding = new Thickness(Settings.MinSpace, Settings.MinSpace + Settings.Border, Settings.MinSpace, Settings.MinSpace),
			BorderThickness = new Thickness(0, 0, 0, Settings.MinBorder),
			FontSize = fontSize.Value,
			FontWeight = FontWeights.Normal,
			LayoutTransform = new ScaleTransform(scale.Value, scale.Value),
			FocusVisualStyle = null,
			ContextMenu = null,
			SnapsToDevicePixels = true,
		};

		ControlHelper.SetCornerRadius(element, new CornerRadius(0));
		TextBoxHelper.SetIsDeleteButtonVisible(element, false);

		if (!string.IsNullOrEmpty(pattern))
		{
			var regex = new Regex(pattern);

			DataObject.AddPastingHandler(element, (s, e) =>
			{
				if (e.DataObject.GetDataPresent(typeof(string)))
				{
					if (!regex.IsMatch(element.Text.Remove(element.SelectionStart, element.SelectionLength).Insert(element.SelectionStart, (string)e.DataObject.GetData(typeof(string)))))
						e.CancelCommand();
				}
				else
					e.CancelCommand();
			});

			element.PreviewKeyDown += (s, e) =>
			{
				if (e.Key == Key.Space && (element.Text.Length == 0 || element.Text.Length == element.SelectionLength))
					e.Handled = true;
			};

			element.PreviewTextInput += (s, e) =>
			{
				e.Handled = !regex.IsMatch(element.Text.Remove(element.SelectionStart, element.SelectionLength).Insert(element.SelectionStart, e.Text));
			};
		}

		return element;
	}

	public static TextBox CreateTextBoxVariant(double? size = null, double? scale = null, double? fontSize = null, string pattern = null)
	{
		var element = CreateTextBox(size, scale, fontSize, pattern);

		element.Background = Settings.SecondaryBackground.WithAlpha(Settings.ColorOpacity2).ToBrush();
		element.Padding = new Thickness(Settings.MinSpace);
		element.BorderThickness = new Thickness(Settings.MinBorder);

		ControlHelper.SetCornerRadius(element, new CornerRadius(Settings.MinRound));

		element.Resources.Add("TextControlBorderThemeThicknessFocused", element.BorderThickness);

		return element;
	}

	public static ComboBox CreateComboBox(IDictionary<Enum, string> items, double? size = null, double? scale = null, double? fontSize = null)
	{
		size ??= Settings.ControlSize;
		scale ??= Settings.ControlScaleCompact2;
		fontSize ??= (Settings.FontSize + Settings.FontSizeCompact) / 2;

		var element = new ComboBox
		{
			MinWidth = 0,
			MinHeight = 0,
			MaxWidth = double.PositiveInfinity,
			MaxHeight = double.PositiveInfinity,
			Height = size.Value,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalContentAlignment = HorizontalAlignment.Center,
			VerticalContentAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			Padding = new Thickness(Settings.MaxSpace, 0, Settings.MaxSpace, Settings.Border),
			FontSize = fontSize.Value,
			FontFamily = Settings.FontFamily,
			FontWeight = FontWeights.Normal,
			DisplayMemberPath = "Value",
			SelectedValuePath = "Key",
			ItemsSource = items,
			SelectedItem = items.First(),
			LayoutTransform = new ScaleTransform(scale.Value, scale.Value),
			FocusVisualStyle = null,
			SnapsToDevicePixels = true,
		};

		ControlHelper.SetCornerRadius(element, new CornerRadius(Settings.MinRound));

		return element;
	}

	public static Button CreateButton(Symbol symbol, double? size = null, double? scale = null, Color? color = null)
	{
		size ??= Settings.ControlSize;
		scale ??= Settings.ControlScale;

		var element = new Button
		{
			Background = Settings.TransperentBrush,
			BorderBrush = Settings.TransperentBrush,
			MinWidth = 0,
			MinHeight = 0,
			MaxWidth = double.PositiveInfinity,
			MaxHeight = double.PositiveInfinity,
			Width = size.Value,
			Height = size.Value,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalContentAlignment = HorizontalAlignment.Center,
			VerticalContentAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			Padding = new Thickness(Settings.MinSpace),
			BorderThickness = new Thickness(0),
			Content = new SymbolIcon(symbol)
			{
				LayoutTransform = new ScaleTransform(size.Value / 35, size.Value / 35),
				Opacity = 0.8,
				SnapsToDevicePixels = true,
			},
			LayoutTransform = new ScaleTransform(scale.Value, scale.Value),
			FocusVisualStyle = null,
			SnapsToDevicePixels = true,
		};

		ControlHelper.SetCornerRadius(element, new CornerRadius(Settings.MinRound));

		if (color == null)
			color = GetSymbolColor(symbol);

		element.Resources.Add("ButtonForeground", color.Value.ToBrush());
		element.Resources.Add("ButtonForegroundPointerOver", color.Value.WithAlpha(Settings.ColorOpacity1).ToBrush());
		element.Resources.Add("ButtonForegroundPressed", color.Value.WithAlpha(Settings.ColorOpacity2).ToBrush());

		return element;
	}

	public static Button CreateButtonVariant(Symbol symbol, double? size = null, double? scale = null, Color? color = null)
	{
		var element = CreateButton(symbol, size, scale, color);

		element.Resources.Add("ButtonBackgroundPointerOver", Settings.TertiaryForeground.WithAlpha(Settings.ColorOpacity3).ToBrush());
		element.Resources.Add("ButtonBackgroundPressed", Settings.TertiaryForeground.WithAlpha((byte)(Settings.ColorOpacity3 / 2)).ToBrush());

		return element;
	}

	public static ExpandButton CreateExpandButton()
	{
		var element = new ExpandButton
		{
			Background = Settings.TransperentBrush,
			BorderBrush = Settings.TransperentBrush,
			MinWidth = 0,
			MinHeight = 0,
			MaxWidth = double.PositiveInfinity,
			MaxHeight = double.PositiveInfinity,
			Width = Settings.ControlSizeCompact,
			Height = Settings.ControlSizeCompact,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalContentAlignment = HorizontalAlignment.Center,
			VerticalContentAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			Padding = new Thickness(Settings.MinSpace),
			BorderThickness = new Thickness(0),
			FocusVisualStyle = null,
			SnapsToDevicePixels = true,
		};

		element.Resources.Add("ButtonBackgroundPointerOver", Settings.TransperentBrush);
		element.Resources.Add("ButtonBackgroundPressed", Settings.TransperentBrush);

		return element;
	}

	public static TextIconButton CreateTextIconButton(Symbol symbol, string text = null, double? size = null, double? scale = null, Color? color = null)
	{
		size ??= Settings.ControlSize;
		scale ??= Settings.ControlScale;

		if (color == null)
			color = GetSymbolColor(symbol);

		var element = new TextIconButton(symbol, text, size.Value, color.Value)
		{
			MinWidth = 0,
			MinHeight = 0,
			MaxWidth = double.PositiveInfinity,
			MaxHeight = double.PositiveInfinity,
			Height = size.Value,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalContentAlignment = HorizontalAlignment.Center,
			VerticalContentAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			Padding = new Thickness(Settings.MinSpace),
			LayoutTransform = new ScaleTransform(scale.Value, scale.Value),
			FocusVisualStyle = null,
			SnapsToDevicePixels = true,
		};

		return element;
	}

	public static TextIconButton CreateTextIconButtonVariant(Symbol? symbol = null, string text = null, double ? size = null, double? scale = null, Color? color = null)
	{
		var element = CreateTextIconButton(symbol ?? Symbol.Placeholder, text, size, scale, color);

		element.IsActive = true;

		if (symbol == null)
		{
			element.IconVisibility = Visibility.Collapsed;
			element.Padding = new Thickness(Settings.MaxSpace, 0, Settings.MaxSpace, 0);
		}

		element.ActiveBackground = Settings.SecondaryBackground.WithAlpha(Settings.ColorOpacity2).ToBrush();
		element.ActiveBorderBrush = Settings.StrokeBrush;

		element.Resources.Add("ButtonBackgroundPointerOver", Settings.SelectBackgroundBrush);
		element.Resources.Add("ButtonBackgroundPressed", Settings.TertiaryBackground.WithAlpha(Settings.ColorOpacity2).ToBrush());

		element.Resources.Add("ButtonBorderBrushPointerOver", Settings.StrokeBrush);
		element.Resources.Add("ButtonBorderBrushPressed", Settings.Stroke.WithAlpha(Settings.ColorOpacity2).ToBrush());

		return element;
	}

	public static ColorButton CreateColorButton(double? size = null, double? scale = null)
	{
		size ??= Settings.ControlSize;
		scale ??= Settings.ControlScale;

		return new ColorButton(size.Value, scale.Value)
		{
			LayoutTransform = new ScaleTransform(scale.Value, scale.Value),
			FocusVisualStyle = null,
			SnapsToDevicePixels = true,
		};
	}

	public static ContextMenu CreateContextMenu()
	{
		var menu = new ContextMenu
		{
			Background = Settings.TertiaryBackgroundBrush,
			BorderBrush = Settings.StrokeBrush,
			BorderThickness = new Thickness(Settings.MinBorder),
			FocusVisualStyle = null,
			SnapsToDevicePixels = true,
		};

		return menu;
	}

	public static MenuItem CreateMenuItem(Symbol? symbol, string text, Action action, Color? color = null)
	{
		var foreground = Settings.ForegroundBrush;

		if (color == null && symbol != null)
			color = GetSymbolColor(symbol.Value);

		if (color != null)
			foreground = color.Value.ToBrush();

		var menuItem = new MenuItem
		{
			Icon = symbol == null ? null : new SymbolIcon(symbol.Value)
			{
				Foreground = foreground,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(0, -Settings.Border, 0, 0),
				RenderTransform = new ScaleTransform(Settings.ControlScaleCompact, Settings.ControlScaleCompact),
				RenderTransformOrigin = new Point(1, 1),
			},
			Header = CreateTextBlock(text, Settings.FontSizeCompact),
			Padding = new Thickness(Settings.Space, Settings.MinSpace, Settings.MaxSpace, Settings.MinSpace),
			BorderThickness = new Thickness(0),
			FocusVisualStyle = null,
			SnapsToDevicePixels = true,
		};

		ControlHelper.SetCornerRadius(menuItem, new CornerRadius(Settings.MinRound));

		menuItem.Click += (s, e) => action();

		return menuItem;
	}

	public static ToolTip CreateToolTip(string text, PlacementMode mode = PlacementMode.Bottom, double horizontalOffset = 0, double verticalOffset = 0)
	{
		var element = new ToolTip
		{
			Background = Settings.TertiaryBackgroundBrush,
			BorderBrush = Settings.StrokeBrush,
			Foreground = Settings.ForegroundBrush,
			Content = GetText(text),
			Placement = mode,
			HorizontalOffset = horizontalOffset,
			VerticalOffset = verticalOffset,
		};

		ControlHelper.SetCornerRadius(element, new CornerRadius(Settings.MinRound));

		return element;
	}

	public static FrameworkElement CreateEmpty()
	{
		return new Rectangle
		{
			MinWidth = Settings.Space,
			MinHeight = Settings.Space,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			Margin = new Thickness(0),
			StrokeThickness = 0,
			Visibility = Visibility.Hidden,
		};
	}
}
