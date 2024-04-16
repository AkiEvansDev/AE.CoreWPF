using System.Resources;
using System.Windows;
using System.Windows.Media;

using ModernWpf;
using ModernWpf.Controls;

using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;

namespace AE.CoreWPF;

public static partial class DisplayHelper
{
	public static SWMColor ToColor(this SDColor color) => SWMColor.FromArgb(color.A, color.R, color.G, color.B);
	public static SDColor ToColor(this SWMColor color) => SDColor.FromArgb(color.A, color.R, color.G, color.B);

	public static SWMColor WithAlpha(this SWMColor color, byte opacity) => SWMColor.FromArgb(opacity, color.R, color.G, color.B);
	public static SDColor WithAlpha(this SDColor color, byte opacity) => SDColor.FromArgb(opacity, color.R, color.G, color.B);

	public static SolidColorBrush ToBrush(this SWMColor color) => new(color);
	public static SolidColorBrush ToBrush(this SDColor color) => new(color.ToColor());

	public static IDisplaySettings Settings { get; private set; }
	private static ResourceManager Resource;

	public static void Init(Application application, IDisplaySettings displaySettings, ResourceManager resourceManager = null)
	{
		Settings = displaySettings;
		Resource = resourceManager;

		ThemeManager.Current.AccentColor = Settings.Accent;
		
		application.Resources.Add("SystemControlPageBackgroundAltHighBrush", Settings.SecondaryBackgroundBrush);
		application.Resources.Add("SystemControlPageTextBaseHighBrush", Settings.ForegroundBrush);
		application.Resources.Add("WindowBorder", Settings.StrokeBrush);

		application.Resources.Add("ContentControlThemeFontFamily", Settings.FontFamily);
		application.Resources.Add("ControlContentThemeFontSize", Settings.FontSize);

		#region ComboBox

		application.Resources.Add("ComboBoxBackground", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("ComboBoxBackgroundPointerOver", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("ComboBoxBackgroundPressed", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("ComboBoxBackgroundDisabled", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("ComboBoxDropDownBackground", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("ComboBoxDropDownBackgroundPointerOver", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("ComboBoxDropDownBackgroundPointerPressed", Settings.TertiaryForegroundBrush);

		application.Resources.Add("ComboBoxForeground", Settings.ForegroundBrush);
		application.Resources.Add("ComboBoxForegroundFocused", Settings.ForegroundBrush);
		application.Resources.Add("ComboBoxDropDownForeground", Settings.ForegroundBrush);
		application.Resources.Add("ComboBoxDropDownGlyphForeground", Settings.ForegroundBrush);
		application.Resources.Add("ComboBoxEditableDropDownGlyphForeground", Settings.ForegroundBrush);
		application.Resources.Add("ComboBoxForegroundPointerOver", Settings.SecondaryForegroundBrush);
		application.Resources.Add("ComboBoxForegroundPressed", Settings.SecondaryForegroundBrush);

		application.Resources.Add("ComboBoxItemForeground", Settings.ForegroundBrush);
		application.Resources.Add("ComboBoxItemForegroundSelected", Settings.ForegroundBrush);
		application.Resources.Add("ComboBoxItemForegroundSelectedUnfocused", Settings.ForegroundBrush);
		application.Resources.Add("ComboBoxItemForegroundPointerOver", Settings.SecondaryForegroundBrush);
		application.Resources.Add("ComboBoxItemForegroundSelectedPointerOver", Settings.SecondaryForegroundBrush);
		application.Resources.Add("ComboBoxItemForegroundDisabled", Settings.TertiaryForegroundBrush);
		application.Resources.Add("ComboBoxItemForegroundSelectedDisabled", Settings.TertiaryForegroundBrush);

		application.Resources.Add("ComboBoxBorderBrush", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("ComboBoxBackgroundBorderBrushFocused", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("ComboBoxDropDownBorderBrush", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("ComboBoxBorderBrushPressed", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("ComboBoxBorderBrushDisabled", Settings.TertiaryForegroundBrush);

		application.Resources.Add("ComboBoxDropdownBorderThickness", new Thickness(Settings.MinBorder));

		#endregion
		#region TextControl

		application.Resources.Add("TextControlForeground", Settings.ForegroundBrush);
		application.Resources.Add("TextControlForegroundPointerOver", Settings.ForegroundBrush);
		application.Resources.Add("TextControlForegroundFocused", Settings.ForegroundBrush);
		application.Resources.Add("TextControlForegroundDisabled", Settings.TertiaryForegroundBrush);

		application.Resources.Add("TextControlBorderBrush", Settings.StrokeBrush);
		application.Resources.Add("TextControlBorderBrushPointerOver", Settings.AccentBrush);
		application.Resources.Add("TextControlBorderBrushFocused", Settings.AccentBrush);
		application.Resources.Add("TextControlBorderBrushDisabled", Settings.TertiaryForegroundBrush);

		application.Resources.Add("TextControlPlaceholderForeground", Settings.SecondaryForegroundBrush);
		application.Resources.Add("TextControlPlaceholderForegroundPointerOver", Settings.SecondaryForegroundBrush);
		application.Resources.Add("TextControlPlaceholderForegroundFocused", Settings.SecondaryForegroundBrush);
		application.Resources.Add("TextControlPlaceholderForegroundDisabled", Settings.TertiaryForegroundBrush);

		application.Resources.Add("TextControlBorderThemeThicknessFocused", new Thickness(0, 0, 0, Settings.MinBorder));

		#endregion
		#region CheckBox

		application.Resources.Add("CheckBoxBackgroundUnchecked", Settings.TransperentBrush);
		application.Resources.Add("CheckBoxCheckBackgroundFillUnchecked", Settings.TransperentBrush);
		application.Resources.Add("CheckBoxForegroundUnchecked", Settings.ForegroundBrush);
		application.Resources.Add("CheckBoxBorderBrushUnchecked", Settings.TertiaryForegroundBrush);
		application.Resources.Add("CheckBoxCheckBackgroundStrokeUnchecked", Settings.TertiaryForegroundBrush);
		application.Resources.Add("CheckBoxCheckGlyphForegroundUnchecked", Settings.ForegroundBrush);

		application.Resources.Add("CheckBoxBackgroundUncheckedPointerOver", Settings.TransperentBrush);
		application.Resources.Add("CheckBoxCheckBackgroundFillUncheckedPointerOver", Settings.TransperentBrush);
		application.Resources.Add("CheckBoxForegroundUncheckedPointerOver", Settings.SecondaryForegroundBrush);
		application.Resources.Add("CheckBoxBorderBrushUncheckedPointerOver", Settings.SecondaryForegroundBrush);
		application.Resources.Add("CheckBoxCheckBackgroundStrokeUncheckedPointerOver", Settings.SecondaryForegroundBrush);
		application.Resources.Add("CheckBoxCheckGlyphForegroundUncheckedPointerOver", Settings.SecondaryForegroundBrush);

		application.Resources.Add("CheckBoxBackgroundUncheckedPressed", Settings.TransperentBrush);
		application.Resources.Add("CheckBoxCheckBackgroundFillUncheckedPressed", Settings.TransperentBrush);
		application.Resources.Add("CheckBoxForegroundUncheckedPressed", Settings.SecondaryForegroundBrush);
		application.Resources.Add("CheckBoxBorderBrushUncheckedPressed", Settings.TertiaryForegroundBrush);
		application.Resources.Add("CheckBoxCheckBackgroundStrokeUncheckedPressed", Settings.TertiaryForegroundBrush);
		application.Resources.Add("CheckBoxCheckGlyphForegroundUncheckedPressed", Settings.SecondaryForegroundBrush);

		application.Resources.Add("CheckBoxBackgroundUncheckedDisabled", Settings.SecondaryBackgroundBrush);
		application.Resources.Add("CheckBoxCheckBackgroundFillUncheckedDisabled", Settings.SecondaryBackgroundBrush);
		application.Resources.Add("CheckBoxForegroundUncheckedDisabled", Settings.TertiaryForegroundBrush);
		application.Resources.Add("CheckBoxBorderBrushUncheckedDisabled", Settings.StrokeBrush);
		application.Resources.Add("CheckBoxCheckBackgroundStrokeUncheckedDisabled", Settings.StrokeBrush);
		application.Resources.Add("CheckBoxCheckGlyphForegroundUncheckedDisabled", Settings.TertiaryForegroundBrush);

		application.Resources.Add("CheckBoxForegroundChecked", Settings.BackgroundBrush);
		application.Resources.Add("CheckBoxCheckGlyphForegroundChecked", Settings.BackgroundBrush);

		application.Resources.Add("CheckBoxForegroundCheckedPointerOver", Settings.SecondaryBackgroundBrush);
		application.Resources.Add("CheckBoxCheckGlyphForegroundCheckedPointerOver", Settings.SecondaryBackgroundBrush);

		application.Resources.Add("CheckBoxForegroundCheckedPressed", Settings.SecondaryBackgroundBrush);
		application.Resources.Add("CheckBoxCheckGlyphForegroundCheckedPressed", Settings.SecondaryBackgroundBrush);

		#endregion
		#region TabControl

		application.Resources.Add("TabViewTopHeaderPadding", new Thickness(Settings.Border, 0, 0, 0));
		application.Resources.Add("TabViewBottomHeaderPadding", new Thickness(0, 0, 0, Settings.Space));
		application.Resources.Add("TabViewHeaderPadding", new Thickness(0));
		application.Resources.Add("TabViewItemHeaderPadding", new Thickness(0));
		application.Resources.Add("TabViewItemSeparatorMargin", new Thickness(0));

		application.Resources.Add("TabViewItemSeparator", Settings.TransperentBrush);

		application.Resources.Add("TabViewItemHeaderBackground", Settings.TertiaryBackground.WithAlpha(Settings.ColorOpacity2).ToBrush());
		application.Resources.Add("TabViewItemHeaderBackgroundPointerOver", Settings.SelectBackgroundBrush);
		application.Resources.Add("TabViewItemHeaderBackgroundSelected", Settings.TertiaryBackgroundBrush);

		#endregion
		#region Button

		application.Resources.Add("ButtonForeground", Settings.ForegroundBrush);
		application.Resources.Add("ButtonForegroundPointerOver", Settings.SecondaryForegroundBrush);
		application.Resources.Add("ButtonForegroundPressed", Settings.SecondaryForegroundBrush);
		application.Resources.Add("ButtonForegroundDisabled", Settings.TertiaryForegroundBrush);

		application.Resources.Add("ButtonBackground", Settings.TransperentBrush);
		application.Resources.Add("ButtonBackgroundPointerOver", Settings.SelectBackgroundBrush);
		application.Resources.Add("ButtonBackgroundPressed", Settings.TertiaryBackground.WithAlpha(Settings.ColorOpacity2).ToBrush());
		application.Resources.Add("ButtonBackgroundDisabled", Settings.TertiaryBackground);

		application.Resources.Add("ButtonBorderBrush", Settings.TransperentBrush);
		application.Resources.Add("ButtonBorderBrushPointerOver", Settings.SelectBackgroundBrush);
		application.Resources.Add("ButtonBorderBrushPressed", Settings.TertiaryBackground.WithAlpha(Settings.ColorOpacity2).ToBrush());
		application.Resources.Add("ButtonBorderBrushDisabled", Settings.TertiaryForegroundBrush);

		application.Resources.Add("CloseButtonBackgroundPointerOver", Settings.ErrorColorBrush);
		application.Resources.Add("CloseButtonBackgroundPressed", Settings.ErrorColor.WithAlpha(Settings.ColorOpacity2).ToBrush());

		#endregion
		#region Scroll

		application.Resources.Add("ScrollBarCornerRadius", new CornerRadius(0));

		application.Resources.Add("ScrollBarBackground", Settings.SecondaryBackgroundBrush);
		application.Resources.Add("ScrollBarForeground", Settings.SecondaryForegroundBrush);

		application.Resources.Add("ScrollBarTrackFill", Settings.SecondaryBackgroundBrush);
		application.Resources.Add("ScrollBarBackgroundPointerOver", Settings.SecondaryBackgroundBrush);
		application.Resources.Add("ScrollBarTrackFillPointerOver", Settings.SecondaryBackgroundBrush);
		application.Resources.Add("ScrollBarTrackStroke", Settings.StrokeBrush);
		application.Resources.Add("ScrollBarBorderBrushPointerOver", Settings.StrokeBrush);
		application.Resources.Add("ScrollBarTrackStrokePointerOver", Settings.StrokeBrush);

		application.Resources.Add("ScrollBarPanningThumbBackground", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("ScrollBarThumbBorderBrush", Settings.TertiaryBackgroundBrush);

		application.Resources.Add("RepeatButtonForeground", Settings.ForegroundBrush);

		#endregion
		#region Menu

		application.Resources.Add("MenuFlyoutThemeMinHeight", 0.0);
		application.Resources.Add("MenuFlyoutPresenterThemePadding", new Thickness(Settings.MinSpace));
		application.Resources.Add("MenuFlyoutSeparatorThemePadding", new Thickness(Settings.Space, Settings.MinSpace, Settings.Space, Settings.MinSpace));
		application.Resources.Add("OverlayCornerRadius", new CornerRadius(Settings.MinRound));

		application.Resources.Add("MenuFlyoutSeparatorBackground", Settings.StrokeBrush);
		application.Resources.Add("MenuFlyoutPresenterBackground", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("MenuFlyoutPresenterBorderBrush", Settings.TertiaryBackgroundBrush);

		application.Resources.Add("MenuBarItemBackgroundPointerOver", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("MenuBarItemBackgroundSelected", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("MenuBarItemBackgroundPressed", Settings.TertiaryBackgroundBrush);

		application.Resources.Add("FlyoutPresenterBackground", Settings.TertiaryBackgroundBrush);
		application.Resources.Add("FlyoutBorderThemeBrush", Settings.TertiaryBackgroundBrush);

		#endregion
	}

	public static SWMColor GetSymbolColor(Symbol symbol)
	{
		return symbol switch
		{
			Symbol.Delete or Symbol.Stop => Settings.ErrorColor,
			Symbol.Play => Settings.SuccessColor,
			Symbol.Folder or Symbol.NewFolder => Settings.WarningColor,
			Symbol.Document => Settings.MessageColor,
			_ => Settings.Foreground,
		};
	}

	public static string GetText(string key, params object[] args)
	{
		if (Resource == null || key == null || !key.StartsWith("&="))
			return key;

		var value = Resource.GetString(key);
		if (value == null)
			return key;

		return string.Format(value, args);
	}
}
