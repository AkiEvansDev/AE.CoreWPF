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
			Padding = new Thickness(Settings.Space, Settings.MinSpace + Settings.Border, Settings.Space, Settings.MinSpace),
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
		element.Padding = new Thickness(Settings.Space, Settings.MinSpace, Settings.Space, Settings.MinSpace);
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

	public static Slider CreateSlider(double min, double max, Orientation orientation = Orientation.Horizontal, double? size = null, double? scale = null)
	{
		var tick = Math.Round((max - min) / 10.0, 1);

		size ??= Math.Min(Math.Max((tick < 1 ? 10 : tick) * Settings.ControlSizeCompact, Settings.ControlSize), Settings.ControlSize * 10);
		scale ??= Settings.ControlScale;

		var element = new Slider
		{
			Minimum = min,
			Maximum = max,
			Orientation = orientation,
			TickFrequency = tick,
			TickPlacement = TickPlacement.BottomRight,
			AutoToolTipPrecision = tick < 1 ? 1 : 0,
			AutoToolTipPlacement = AutoToolTipPlacement.TopLeft,
			Margin = new Thickness(0),
			Padding = new Thickness(0),
			LayoutTransform = new ScaleTransform(scale.Value, scale.Value),
			FocusVisualStyle = null,
			ContextMenu = null,
			SnapsToDevicePixels = true,
		};

		if (orientation == Orientation.Horizontal)
			element.Width = size.Value;
		else if (orientation == Orientation.Vertical)
			element.Height = size.Value;

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

		element.Resources.Add("ButtonBackgroundPointerOver", Settings.Foreground.WithAlpha(Settings.ColorOpacity5).ToBrush());
		element.Resources.Add("ButtonBackgroundPressed", Settings.Foreground.WithAlpha(Settings.ColorOpacity4).ToBrush());

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

	public static TextIconButton CreateTextIconButtonVariant(Symbol? symbol = null, string text = null, double? size = null, double? scale = null, Color? color = null)
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
		element.Resources.Add("ButtonBackgroundPressed", Settings.Foreground.WithAlpha(Settings.ColorOpacity4).ToBrush());

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

	public static ScrollViewerEx CreateScroll(ScrollBarVisibility horizontal = ScrollBarVisibility.Hidden, ScrollBarVisibility vertical = ScrollBarVisibility.Hidden)
	{
		return new ScrollViewerEx
		{
			BorderBrush = Settings.StrokeBrush,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			Margin = new Thickness(0),
			Padding = new Thickness(0),
			BorderThickness = new Thickness(0),
			HorizontalScrollBarVisibility = horizontal,
			VerticalScrollBarVisibility = vertical,
		};
	}

	public static Menu CreateMenu()
	{
		var menu = new Menu
		{
			Height = Settings.ControlSizeCompact2,
			Background = Settings.SecondaryBackgroundBrush,
			Padding = new Thickness(0),
			BorderThickness = new Thickness(0),
			FocusVisualStyle = null,
			SnapsToDevicePixels = true,
		};

		ControlHelper.SetCornerRadius(menu, new CornerRadius(0));

		return menu;
	}

	public static ContextMenu CreateContextMenu()
	{
		var menu = new ContextMenu
		{
			Background = Settings.TertiaryBackgroundBrush,
			BorderThickness = new Thickness(Settings.MinBorder),
			FocusVisualStyle = null,
			SnapsToDevicePixels = true,
		};

		return menu;
	}

	public static MenuItem CreateMenuItem(string text, Symbol? symbol = null, Action action = null, double? fontSize = null, double? height = null, double iconRotate = 0, Color ? color = null)
	{
		return CreateMenuItem(text, symbol, action, null, fontSize, height, iconRotate, color);
	}

	public static MenuItem CreateSubMenuItem(Symbol? symbol, string text, Action action = null, string inputGesture = null, double iconRotate = 0, Color? color = null)
	{
		var menuItem = CreateMenuItem(text, symbol, action, inputGesture, Settings.FontSizeCompact, double.NaN, iconRotate, color);

		menuItem.BorderThickness = new Thickness(0);

		return menuItem;
	}

	private static MenuItem CreateMenuItem(string text, Symbol? symbol = null, Action action = null, string inputGesture = null, double? fontSize = null, double? height = null, double iconRotate = 0, Color? color = null)
	{
		var foreground = Settings.ForegroundBrush;

		if (color == null && symbol != null)
			color = GetSymbolColor(symbol.Value);

		if (color != null)
			foreground = color.Value.ToBrush();

		var textItem = CreateTextBlock(text, fontSize ?? Settings.FontSizeCompact2);
		var inputGestureItem = string.IsNullOrEmpty(inputGesture) 
			? null 
			: CreateTertiaryTextBlock(inputGesture, Settings.FontSizeCompact);

		var header = new Grid();

		header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
		header.Children.Add(textItem);

		if (inputGestureItem != null)
		{
			header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(Settings.MaxSpace * 2, GridUnitType.Pixel) });
			header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

			Grid.SetColumn(inputGestureItem, 2);

			header.Children.Add(inputGestureItem);
		}

		var menuItem = new MenuItem
		{
			Height = height ?? Settings.ControlSizeCompact2,
			BorderBrush = Settings.TransperentBrush,
			Icon = symbol == null ? null : new SymbolIcon(symbol.Value)
			{
				Foreground = foreground,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(0, -Settings.Border, 0, 0),
				LayoutTransform = new RotateTransform(iconRotate),
				RenderTransform = new ScaleTransform(Settings.ControlScaleCompact2, Settings.ControlScaleCompact2),
				RenderTransformOrigin = new Point(1, 1),
			},
			Header = header,
			Padding = new Thickness(Settings.MaxSpace, Settings.Space, Settings.MaxSpace, Settings.Space),
			BorderThickness = new Thickness(Settings.MinBorder, Settings.MinBorder, Settings.MinBorder, 0),
			FocusVisualStyle = null,
			SnapsToDevicePixels = true,
		};

		menuItem.IsEnabledChanged += (s, e) =>
		{
			if (e.NewValue is bool isEnabled)
			{
				if (menuItem.Icon != null && menuItem.Icon is SymbolIcon icon)
					icon.Opacity = isEnabled ? 1 : Settings.ControlDisabledOpacity;

				textItem.Opacity = isEnabled ? 1 : Settings.ControlDisabledOpacity;

				if (inputGestureItem != null)
					inputGestureItem.Opacity = textItem.Opacity;
			}
		};

		ControlHelper.SetCornerRadius(menuItem, new CornerRadius(0));

		if (action != null)
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
