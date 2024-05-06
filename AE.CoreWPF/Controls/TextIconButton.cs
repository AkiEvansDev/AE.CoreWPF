using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;

namespace AE.CoreWPF.Controls;

public class TextIconButton : Button
{
	private bool isActive;
	public bool IsActive
	{
		get => isActive;
		set
		{
			isActive = value;
			RefreshState();
		}
	}

	public Visibility IconVisibility
	{
		get => icon.Visibility;
		set => icon.Visibility = value;
	}

	public Symbol Symbol
	{
		get => icon.Symbol;
		set => icon.Symbol = value;
	}

	public string Text
	{
		get => textBlock.Text;
		set => textBlock.Text = value;
	}

	private Brush activeBackground;
	public Brush ActiveBackground
	{
		get => activeBackground;
		set
		{
			activeBackground = value;
			RefreshState();
		}
	}

	private Brush activeBorderBrush;
	public Brush ActiveBorderBrush
	{
		get => activeBorderBrush;
		set
		{
			activeBorderBrush = value;
			RefreshState();
		}
	}

	private SymbolIcon icon;
	private TextBlock textBlock;

	public TextIconButton(Symbol symbol, string text, double size, Color color)
	{
		Layout(symbol, text, size, color);
		RefreshState();
	}

	private void Layout(Symbol symbol, string text, double size, Color color)
	{
		Style = (Style)Application.Current.Resources["DefaultButtonStyle"];

		activeBackground = DisplayHelper.Settings.SelectBackgroundBrush;
		activeBorderBrush = DisplayHelper.Settings.AccentBrush;

		BorderBrush = DisplayHelper.Settings.TransperentBrush;
		BorderThickness = new Thickness(DisplayHelper.Settings.MinBorder);

		ControlHelper.SetCornerRadius(this, new CornerRadius(DisplayHelper.Settings.MinRound));

		Resources.Add("ButtonForeground", color.ToBrush());
		Resources.Add("ButtonForegroundPointerOver", color.WithAlpha(DisplayHelper.Settings.ColorOpacity1).ToBrush());
		Resources.Add("ButtonForegroundPressed", color.WithAlpha(DisplayHelper.Settings.ColorOpacity2).ToBrush());

		var panel = new SimpleStackPanel
		{
			Orientation = Orientation.Horizontal,
		};

		icon = new SymbolIcon(symbol)
		{
			Margin = new Thickness(DisplayHelper.Settings.MinSpace, 0, 0, 0),
			LayoutTransform = new ScaleTransform(size / 35, size / 35),
			SnapsToDevicePixels = true,
		};

		textBlock = DisplayHelper.CreateTextBlock(text, size / 2)
			.SetTextPadding(DisplayHelper.Settings.Space, 0);

		panel.Children.Add(icon);
		panel.Children.Add(textBlock);

		Content = panel;

		IsEnabledChanged += OnIsEnabledChanged;
	}

	private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		icon.Foreground = IsEnabled
			? Foreground
			: DisplayHelper.Settings.TertiaryForegroundBrush;

		textBlock.Foreground = IsEnabled
			? DisplayHelper.Settings.ForegroundBrush
			: DisplayHelper.Settings.TertiaryForegroundBrush;
	}

	private void RefreshState()
	{
		Background = IsActive
			? ActiveBackground
			: DisplayHelper.Settings.TransperentBrush;

		BorderBrush = IsActive
			? ActiveBorderBrush
			: DisplayHelper.Settings.TransperentBrush;
	}
}
