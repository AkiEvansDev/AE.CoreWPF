
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;


namespace AE.CoreWPF.Controls;

public static partial class DisplayHelper
{
	public const int CONTROL_SIZE = 28;
	public const double CONTROL_SCALE = 1;

	public const int CONTROL_SIZE_COMPACT = 20;
	public const double CONTROL_SCALE_COMPACT = 0.8;

	public const int CONTROL_SIZE_COMPACT_1 = 22;
	public const double CONTROL_SCALE_COMPACT_1 = 0.84;

	public const int CONTROL_SIZE_COMPACT_2 = 24;
	public const double CONTROL_SCALE_COMPACT_2 = 0.88;

	public static TextBlock CreateTextBlock(string text = null, int fontSize = 14, FontWeight? fontWeight = null, double opacity = 1)
	{
		return new TextBlock
		{
			Foreground = Settings.Foreground.ToBrush(),
			Text = GetText(text),
			TextAlignment = TextAlignment.Left,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			Padding = new Thickness(0),
			FontSize = fontSize,
			FontWeight = fontWeight ?? FontWeights.Normal,
			Opacity = opacity,
		};
	}

	public static CheckBox CreateCheckBox(double size = CONTROL_SIZE, double scale = CONTROL_SCALE)
	{
		var element = new CheckBox
		{
			MinWidth = 0,
			MinHeight = 0,
			MaxWidth = double.PositiveInfinity,
			MaxHeight = double.PositiveInfinity,
			Width = size,
			Height = size,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			Padding = new Thickness(Settings.MinSpace),
			LayoutTransform = new ScaleTransform(scale, scale),
			FocusVisualStyle = null,
		};

		ControlHelper.SetCornerRadius(element, new CornerRadius(Settings.Round));

		return element;
	}

	public static TextBox CreateTextBox(double size = CONTROL_SIZE, double scale = CONTROL_SCALE, string pattern = "[^a-zA-Z#0-9]+")
	{
		var element = new TextBox
		{
			Background = Settings.Transperent.ToBrush(),
			Foreground = Settings.Foreground.ToBrush(),
			MinWidth = 0,
			MinHeight = 0,
			MaxWidth = double.PositiveInfinity,
			MaxHeight = double.PositiveInfinity,
			Height = size,
			TextAlignment = TextAlignment.Left,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalContentAlignment = HorizontalAlignment.Left,
			VerticalContentAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			Padding = new Thickness(Settings.MinSpace),
			BorderThickness = new Thickness(0, 0, 0, Settings.MinBorder),
			FontSize = size / 2,
			LayoutTransform = new ScaleTransform(scale, scale),
			FocusVisualStyle = null,
			ContextMenu = null,
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
					if (regex.IsMatch((string)e.DataObject.GetData(typeof(string))))
						e.CancelCommand();
				}
				else
					e.CancelCommand();
			});

			element.PreviewTextInput += (s, e) =>
			{
				e.Handled = regex.IsMatch(e.Text);
			};
		}

		return element;
	}

	public static ComboBox CreateComboBox(IDictionary<Enum, string> items, double size = CONTROL_SIZE, double scale = CONTROL_SCALE)
	{
		var element = new ComboBox
		{
			MinWidth = 0,
			MinHeight = 0,
			MaxWidth = double.PositiveInfinity,
			MaxHeight = double.PositiveInfinity,
			Height = size,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalContentAlignment = HorizontalAlignment.Center,
			VerticalContentAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			FontSize = size / 2 - 2,
			FontWeight = FontWeights.Medium,
			DisplayMemberPath = "Value",
			SelectedValuePath = "Key",
			ItemsSource = items,
			SelectedItem = items.First(),
			LayoutTransform = new ScaleTransform(scale, scale),
			FocusVisualStyle = null,
		};

		ControlHelper.SetCornerRadius(element, new CornerRadius(0));

		return element;
	}

	public static Button CreateButton(Symbol symbol, double size = CONTROL_SIZE, double scale = CONTROL_SCALE, Color? color = null)
	{
		var element = new Button
		{
			Background = Settings.TransperentBrush,
			BorderBrush = Settings.TransperentBrush,
			MinWidth = 0,
			MinHeight = 0,
			MaxWidth = double.PositiveInfinity,
			MaxHeight = double.PositiveInfinity,
			Width = size,
			Height = size,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalContentAlignment = HorizontalAlignment.Center,
			VerticalContentAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			Padding = new Thickness(Settings.MinSpace),
			BorderThickness = new Thickness(0),
			Content = new SymbolIcon(symbol)
			{
				LayoutTransform = new ScaleTransform(size / 35, size / 35),
				Opacity = 0.8,
			},
			LayoutTransform = new ScaleTransform(scale, scale),
			FocusVisualStyle = null,
		};

		ControlHelper.SetCornerRadius(element, new CornerRadius(0));

		if (color == null)
			color = GetSymbolColor(symbol);

		if (color != null)
		{
			var foreground = color.Value.ToBrush();
			var secondaryForeground = color.Value.WithAlpha(200).ToBrush();

			element.Resources.Add("ButtonForeground", foreground);
			element.Resources.Add("ButtonForegroundPointerOver", secondaryForeground);
			element.Resources.Add("ButtonForegroundPressed", secondaryForeground);
		}

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
			Width = CONTROL_SIZE_COMPACT,
			Height = CONTROL_SIZE_COMPACT,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalContentAlignment = HorizontalAlignment.Center,
			VerticalContentAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			Padding = new Thickness(Settings.MinSpace),
			BorderThickness = new Thickness(0),
			FocusVisualStyle = null,
		};

		element.Resources.Add("ButtonBackgroundPointerOver", Settings.TransperentBrush);
		element.Resources.Add("ButtonBackgroundPressed", Settings.TransperentBrush);

		return element;
	}

	public static TextIconButton CreateTextIconButton(Symbol symbol, string text = null, double size = CONTROL_SIZE, double scale = CONTROL_SCALE, Color? color = null)
	{
		if (color == null)
			color = GetSymbolColor(symbol);

		var element = new TextIconButton(symbol, text, size, color)
		{
			MinWidth = 0,
			MinHeight = 0,
			MaxWidth = double.PositiveInfinity,
			MaxHeight = double.PositiveInfinity,
			Height = size,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalContentAlignment = HorizontalAlignment.Center,
			VerticalContentAlignment = VerticalAlignment.Center,
			Margin = new Thickness(0),
			Padding = new Thickness(Settings.MinSpace),
			LayoutTransform = new ScaleTransform(scale, scale),
			FocusVisualStyle = null,
		};

		return element;
	}

	public static ColorButton CreateColorButton(double size = CONTROL_SIZE, double scale = CONTROL_SCALE)
	{
		return new ColorButton(size, scale)
		{
			LayoutTransform = new ScaleTransform(scale, scale),
			FocusVisualStyle = null,
		};
	}

	public static ContextMenu CreateContextMenu()
	{
		var menu = new ContextMenu
		{
			Background = Settings.TertiaryBackground.ToBrush(),
			BorderBrush = Settings.SelectStroke.ToBrush(),
			BorderThickness = new Thickness(Settings.MinBorder),
			FocusVisualStyle = null,
		};

		return menu;
	}

	public static MenuItem CreateMenuItem(Symbol? symbol, string text, Action action, Color? color = null)
	{
		var foreground = Settings.Foreground.ToBrush();

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
				RenderTransform = new ScaleTransform(CONTROL_SCALE_COMPACT, CONTROL_SCALE_COMPACT),
				RenderTransformOrigin = new Point(1, 1),
			},
			Header = CreateTextBlock(text, 12),
			Padding = new Thickness(Settings.Space, Settings.MinSpace, Settings.Space, Settings.MinSpace),
			BorderThickness = new Thickness(0),
			FocusVisualStyle = null,
		};

		ControlHelper.SetCornerRadius(menuItem, new CornerRadius(0));

		menuItem.Click += (s, e) => action();

		return menuItem;
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
