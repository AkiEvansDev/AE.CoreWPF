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

	private SymbolIcon icon;
	private TextBlock textBlock;

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

	public TextIconButton(Symbol symbol, string text, double size, Color? color = null)
	{
		Layout(symbol, text, size, color);
		RefreshState();
	}

	private void Layout(Symbol symbol, string text, double size, Color? color)
	{
		Style = (Style)Application.Current.Resources["DefaultButtonStyle"];

		BorderBrush = DisplayHelper.Settings.TransperentBrush;
		BorderThickness = new Thickness(DisplayHelper.Settings.MinBorder);


		ControlHelper.SetCornerRadius(this, new CornerRadius(0));

		var panel = new SimpleStackPanel
		{
			Spacing = DisplayHelper.Settings.Space,
			Orientation = Orientation.Horizontal,
		};

		icon = new SymbolIcon(symbol)
		{
			LayoutTransform = new ScaleTransform(size / 35, size / 35),
			Opacity = 0.8,
		};

		SolidColorBrush foreground = null;
		SolidColorBrush secondaryForeground = null;

		if (color != null)
		{
			foreground = color.Value.ToBrush();
			secondaryForeground = color.Value.WithAlpha(200).ToBrush();
		}

		if (foreground != null && secondaryForeground != null)
		{
			Resources.Add("ButtonForeground", foreground);
			Resources.Add("ButtonForegroundPointerOver", secondaryForeground);
			Resources.Add("ButtonForegroundPressed", secondaryForeground);
		}

		textBlock = DisplayHelper.CreateTextBlock(text, fontSize: (int)(size / 2));
		textBlock.Padding = new Thickness(0, 0, DisplayHelper.Settings.Space, DisplayHelper.Settings.Border);

		panel.Children.Add(icon);
		panel.Children.Add(textBlock);

		Content = panel;

		IsEnabledChanged += OnIsEnabledChanged;
	}

	private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		icon.Foreground = IsEnabled
			? (SolidColorBrush)Resources["ButtonForeground"]
			: DisplayHelper.Settings.TertiaryForegroundBrush;

		textBlock.Foreground = IsEnabled
			? DisplayHelper.Settings.ForegroundBrush
			: DisplayHelper.Settings.TertiaryForegroundBrush;
	}

	private void RefreshState()
	{
		Background = IsActive
			? DisplayHelper.Settings.TertiaryBackground.WithAlpha(200).ToBrush()
			: DisplayHelper.Settings.TransperentBrush;

		BorderBrush = IsActive
			? DisplayHelper.Settings.AccentBrush
			: DisplayHelper.Settings.TransperentBrush;
	}
}
