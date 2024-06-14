using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;

namespace AE.CoreWPF;

public static partial class DisplayHelper
{
	public static T SetBackground<T>(this T element, Brush brush) where T : Control
	{
		element.Background = brush;
		return element;
	}

	public static T SetForeground<T>(this T element, Brush brush) where T : Control
	{
		element.Foreground = brush;
		return element;
	}

	public static T SetTextForeground<T>(this T element, Brush brush) where T : TextBlock
	{
		element.Foreground = brush;
		return element;
	}

	public static T SetHorizontalAlignment<T>(this T element, HorizontalAlignment alignment) where T : FrameworkElement
	{
		element.HorizontalAlignment = alignment;
		return element;
	}

	public static T SetVerticalAlignment<T>(this T element, VerticalAlignment alignment) where T : FrameworkElement
	{
		element.VerticalAlignment = alignment;
		return element;
	}

	public static T SetMargin<T>(this T element, double uniformLength) where T : FrameworkElement
	{
		element.Margin = new Thickness(uniformLength);
		return element;
	}

	public static T SetMargin<T>(this T element, double leftRight, double topBottom) where T : FrameworkElement
	{
		element.Margin = new Thickness(leftRight, topBottom, leftRight, topBottom);
		return element;
	}

	public static T SetMargin<T>(this T element, double left, double top, double right, double bottom) where T : FrameworkElement
	{
		element.Margin = new Thickness(left, top, right, bottom);
		return element;
	}

	public static T SetPadding<T>(this T element, double uniformLength) where T : Control
	{
		element.Padding = new Thickness(uniformLength);
		return element;
	}

	public static T SetPadding<T>(this T element, double leftRight, double topBottom) where T : Control
	{
		element.Padding = new Thickness(leftRight, topBottom, leftRight, topBottom);
		return element;
	}

	public static T SetPadding<T>(this T element, double left, double top, double right, double bottom) where T : Control
	{
		element.Padding = new Thickness(left, top, right, bottom);
		return element;
	}

	public static T SetTextPadding<T>(this T element, double uniformLength) where T : TextBlock
	{
		element.Padding = new Thickness(uniformLength);
		return element;
	}

	public static T SetTextPadding<T>(this T element, double leftRight, double topBottom) where T : TextBlock
	{
		element.Padding = new Thickness(leftRight, topBottom, leftRight, topBottom);
		return element;
	}

	public static T SetTextPadding<T>(this T element, double left, double top, double right, double bottom) where T : TextBlock
	{
		element.Padding = new Thickness(left, top, right, bottom);
		return element;
	}

	public static T SetBorder<T>(this T element, double uniformLength) where T : Control
	{
		element.BorderThickness = new Thickness(uniformLength);
		return element;
	}

	public static T SetBorder<T>(this T element, double leftRight, double topBottom) where T : Control
	{
		element.BorderThickness = new Thickness(leftRight, topBottom, leftRight, topBottom);
		return element;
	}

	public static T SetBorder<T>(this T element, double left, double top, double right, double bottom) where T : Control
	{
		element.BorderThickness = new Thickness(left, top, right, bottom);
		return element;
	}

	public static T SetBorderBrush<T>(this T element, Brush brush) where T : Control
	{
		element.BorderBrush = brush;
		return element;
	}

	public static T SetVisibility<T>(this T element, Visibility visibility) where T : UIElement
	{
		element.Visibility = visibility;
		return element;
	}

	public static T SetOpacity<T>(this T element, double opacity) where T : UIElement
	{
		element.Opacity = opacity;
		return element;
	}

	public static T SetCornerRadius<T>(this T element, double uniformLength) where T : Control
	{
		ControlHelper.SetCornerRadius(element, new CornerRadius(uniformLength));
		return element;
	}

	public static T SetCornerRadius<T>(this T element, double topLeftBottomRight, double topRightBottomLeft) where T : Control
	{
		ControlHelper.SetCornerRadius(element, new CornerRadius(topLeftBottomRight, topRightBottomLeft, topLeftBottomRight, topRightBottomLeft));
		return element;
	}

	public static T SetCornerRadius<T>(this T element, double topLeft, double topRight, double bottomRight, double bottomLeft) where T : Control
	{
		ControlHelper.SetCornerRadius(element, new CornerRadius(topLeft, topRight, bottomRight, bottomLeft));
		return element;
	}

    #region Button

    public static T SetButtonBackground<T>(this T element, Brush brush) where T : Button
    {
        return SetResources(element, "ButtonBackground", brush);
    }

    public static T SetButtonBackgroundPointerOver<T>(this T element, Brush brush) where T : Button
	{
		return SetResources(element, "ButtonBackgroundPointerOver", brush);
	}

	public static T SetButtonBackgroundPressed<T>(this T element, Brush brush) where T : Button
	{
		return SetResources(element, "ButtonBackgroundPressed", brush);
	}

    public static T SetButtonBackgroundDisabled<T>(this T element, Brush brush) where T : Button
    {
        return SetResources(element, "ButtonBackgroundDisabled", brush);
    }

    public static T SetButtonForeground<T>(this T element, Brush brush) where T : Button
	{
		return SetResources(element, "ButtonForeground", brush);
	}

	public static T SetButtonForegroundPointerOver<T>(this T element, Brush brush) where T : Button
	{
		return SetResources(element, "ButtonForegroundPointerOver", brush);
	}

	public static T SetButtonForegroundPressed<T>(this T element, Brush brush) where T : Button
	{
		return SetResources(element, "ButtonForegroundPressed", brush);
	}

    public static T SetButtonForegroundDisabled<T>(this T element, Brush brush) where T : Button
    {
        return SetResources(element, "ButtonForegroundDisabled", brush);
    }

    public static T SetButtonBorderBrush<T>(this T element, Brush brush) where T : Button
    {
        return SetResources(element, "ButtonBorderBrush", brush);
    }

    public static T SetButtonBorderBrushPointerOver<T>(this T element, Brush brush) where T : Button
    {
        return SetResources(element, "ButtonBorderBrushPointerOver", brush);
    }

    public static T SetButtonBorderBrushPressed<T>(this T element, Brush brush) where T : Button
    {
        return SetResources(element, "ButtonBorderBrushPressed", brush);
    }

    public static T SetButtonBorderBrushDisabled<T>(this T element, Brush brush) where T : Button
    {
        return SetResources(element, "ButtonBorderBrushDisabled", brush);
    }

    #endregion
    #region TextControl

    public static T SetTextBoxForeground<T>(this T element, Brush brush) where T : TextBox
    {
        return SetResources(element, "TextControlForeground", brush);
    }

    public static T SetNumberBoxForeground<T>(this T element, Brush brush) where T : NumberBox
    {
        return SetResources(element, "TextControlForeground", brush);
    }

    public static T SetTextBoxForegroundPointerOver<T>(this T element, Brush brush) where T : TextBox
    {
        return SetResources(element, "TextControlForegroundPointerOver", brush);
    }

    public static T SetNumberBoxForegroundPointerOver<T>(this T element, Brush brush) where T : NumberBox
    {
        return SetResources(element, "TextControlForegroundPointerOver", brush);
    }

    public static T SetTextBoxForegroundFocused<T>(this T element, Brush brush) where T : TextBox
    {
        return SetResources(element, "TextControlForegroundFocused", brush);
    }

    public static T SetNumberBoxForegroundFocused<T>(this T element, Brush brush) where T : NumberBox
    {
        return SetResources(element, "TextControlForegroundFocused", brush);
    }

    public static T SetTextBoxForegroundDisabled<T>(this T element, Brush brush) where T : TextBox
    {
        return SetResources(element, "TextControlForegroundDisabled", brush);
    }

    public static T SetNumberBoxForegroundDisabled<T>(this T element, Brush brush) where T : NumberBox
    {
        return SetResources(element, "TextControlForegroundDisabled", brush);
    }

    public static T SetTextBoxBorderBrush<T>(this T element, Brush brush) where T : TextBox
    {
        return SetResources(element, "TextControlBorderBrush", brush);
    }

    public static T SetNumberBoxBorderBrush<T>(this T element, Brush brush) where T : NumberBox
    {
        return SetResources(element, "TextControlBorderBrush", brush);
    }

    public static T SetTextBoxBorderBrushPointerOver<T>(this T element, Brush brush) where T : TextBox
    {
        return SetResources(element, "TextControlBorderBrushPointerOver", brush);
    }

    public static T SetNumberBoxBorderBrushPointerOver<T>(this T element, Brush brush) where T : NumberBox
    {
        return SetResources(element, "TextControlBorderBrushPointerOver", brush);
    }

    public static T SetTextBoxBorderBrushFocused<T>(this T element, Brush brush) where T : TextBox
    {
        return SetResources(element, "TextControlBorderBrushFocused", brush);
    }

    public static T SetNumberBoxBorderBrushFocused<T>(this T element, Brush brush) where T : NumberBox
    {
        return SetResources(element, "TextControlBorderBrushFocused", brush);
    }

    public static T SetTextBoxBorderBrushDisabled<T>(this T element, Brush brush) where T : TextBox
    {
        return SetResources(element, "TextControlBorderBrushDisabled", brush);
    }

    public static T SetNumberBoxBorderBrushDisabled<T>(this T element, Brush brush) where T : NumberBox
    {
        return SetResources(element, "TextControlBorderBrushDisabled", brush);
    }

    public static T SetTextBoxPlaceholderForeground<T>(this T element, Brush brush) where T : TextBox
    {
        return SetResources(element, "TextControlPlaceholderForeground", brush);
    }

    public static T SetNumberBoxPlaceholderForeground<T>(this T element, Brush brush) where T : NumberBox
    {
        return SetResources(element, "TextControlPlaceholderForeground", brush);
    }

    public static T SetTextBoxPlaceholderForegroundPointerOver<T>(this T element, Brush brush) where T : TextBox
    {
        return SetResources(element, "TextControlPlaceholderForegroundPointerOver", brush);
    }

    public static T SetNumberBoxPlaceholderForegroundPointerOver<T>(this T element, Brush brush) where T : NumberBox
    {
        return SetResources(element, "TextControlPlaceholderForegroundPointerOver", brush);
    }

    public static T SetTextBoxPlaceholderForegroundFocused<T>(this T element, Brush brush) where T : TextBox
    {
        return SetResources(element, "TextControlPlaceholderForegroundFocused", brush);
    }

    public static T SetNumberBoxPlaceholderForegroundFocused<T>(this T element, Brush brush) where T : NumberBox
    {
        return SetResources(element, "TextControlPlaceholderForegroundFocused", brush);
    }

    public static T SetTextBoxPlaceholderForegroundDisabled<T>(this T element, Brush brush) where T : TextBox
    {
        return SetResources(element, "TextControlPlaceholderForegroundDisabled", brush);
    }

    public static T SetNumberBoxPlaceholderForegroundDisabled<T>(this T element, Brush brush) where T : NumberBox
    {
        return SetResources(element, "TextControlPlaceholderForegroundDisabled", brush);
    }

    public static T SetTextBoxBorderThicknessFocused<T>(this T element, Thickness thickness) where T : TextBox
    {
        return SetResources(element, "TextControlBorderThemeThicknessFocused", thickness);
    }

    public static T SetNumberBoxBorderThicknessFocused<T>(this T element, Thickness thickness) where T : NumberBox
    {
        return SetResources(element, "TextControlBorderThemeThicknessFocused", thickness);
    }

    #endregion
    #region ComboBox

    public static T SetComboBoxBackground<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxBackground", brush);
    }

    public static T SetComboBoxBackgroundPointerOver<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxBackgroundPointerOver", brush);
    }

    public static T SetComboBoxBackgroundPressed<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxBackgroundPressed", brush);
    }

    public static T SetComboBoxBackgroundDisabled<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxBackgroundDisabled", brush);
    }

    public static T SetComboBoxDropDownBackground<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxDropDownBackground", brush);
    }

    public static T SetComboBoxDropDownBackgroundPointerOver<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxDropDownBackgroundPointerOver", brush);
    }

    public static T SetComboBoxDropDownBackgroundPointerPressed<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxDropDownBackgroundPointerPressed", brush);
    }

    public static T SetComboBoxForeground<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxForeground", brush);
    }

    public static T SetComboBoxForegroundFocused<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxForegroundFocused", brush);
    }

    public static T SetComboBoxDropDownForeground<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxDropDownForeground", brush);
    }

    public static T SetComboBoxDropDownGlyphForeground<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxDropDownGlyphForeground", brush);
    }

    public static T SetComboBoxEditableDropDownGlyphForeground<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxEditableDropDownGlyphForeground", brush);
    }

    public static T SetComboBoxForegroundPointerOver<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxForegroundPointerOver", brush);
    }

    public static T SetComboBoxForegroundPressed<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxForegroundPressed", brush);
    }

    public static T SetComboBoxItemForeground<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxItemForeground", brush);
    }

    public static T SetComboBoxItemForegroundSelected<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxItemForegroundSelected", brush);
    }

    public static T SetComboBoxItemForegroundSelectedUnfocused<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxItemForegroundSelectedUnfocused", brush);
    }

    public static T SetComboBoxItemForegroundPointerOver<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxItemForegroundPointerOver", brush);
    }

    public static T SetComboBoxItemForegroundSelectedPointerOver<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxItemForegroundSelectedPointerOver", brush);
    }

    public static T SetComboBoxItemForegroundDisabled<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxItemForegroundDisabled", brush);
    }

    public static T SetComboBoxItemForegroundSelectedDisabled<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxItemForegroundSelectedDisabled", brush);
    }

    public static T SetComboBoxBorderBrush<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxBorderBrush", brush);
    }

    public static T SetComboBoxBackgroundBorderBrushFocused<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxBackgroundBorderBrushFocused", brush);
    }

    public static T SetComboBoxDropDownBorderBrush<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxDropDownBorderBrush", brush);
    }

    public static T SetComboBoxBorderBrushPressed<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxBorderBrushPressed", brush);
    }

    public static T SetComboBoxBorderBrushDisabled<T>(this T element, Brush brush) where T : ComboBox
    {
        return SetResources(element, "ComboBoxBorderBrushDisabled", brush);
    }

    public static T SetComboBoxDropdownBorderThickness<T>(this T element, Thickness thickness) where T : ComboBox
    {
        return SetResources(element, "ComboBoxDropdownBorderThickness", thickness);
    }

    #endregion
    #region CheckBox

    public static T SetCheckBoxCheckBackgroundFillChecked<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundFillChecked", brush);
    }

    public static T SetCheckBoxCheckBackgroundStrokeChecked<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundStrokeChecked", brush);
    }

    public static T SetCheckBoxCheckBackgroundFillCheckedPointerOver<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundFillCheckedPointerOver", brush);
    }

    public static T SetCheckBoxCheckBackgroundStrokeCheckedPointerOver<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundStrokeCheckedPointerOver", brush);
    }

    public static T SetCheckBoxCheckBackgroundFillCheckedPressed<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundFillCheckedPressed", brush);
    }

    public static T SetCheckBoxCheckBackgroundStrokeCheckedPressed<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundStrokeCheckedPressed", brush);
    }

    public static T SetCheckBoxBackgroundUnchecked<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxBackgroundUnchecked", brush);
    }

    public static T SetCheckBoxCheckBackgroundFillUnchecked<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundFillUnchecked", brush);
    }

    public static T SetCheckBoxForegroundUnchecked<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxForegroundUnchecked", brush);
    }

    public static T SetCheckBoxBorderBrushUnchecked<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxBorderBrushUnchecked", brush);
    }

    public static T SetCheckBoxCheckBackgroundStrokeUnchecked<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundStrokeUnchecked", brush);
    }

    public static T SetCheckBoxCheckGlyphForegroundUnchecked<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckGlyphForegroundUnchecked", brush);
    }

    public static T SetCheckBoxBackgroundUncheckedPointerOver<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxBackgroundUncheckedPointerOver", brush);
    }

    public static T SetCheckBoxCheckBackgroundFillUncheckedPointerOver<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundFillUncheckedPointerOver", brush);
    }

    public static T SetCheckBoxForegroundUncheckedPointerOver<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxForegroundUncheckedPointerOver", brush);
    }

    public static T SetCheckBoxBorderBrushUncheckedPointerOver<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxBorderBrushUncheckedPointerOver", brush);
    }

    public static T SetCheckBoxCheckBackgroundStrokeUncheckedPointerOver<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundStrokeUncheckedPointerOver", brush);
    }

    public static T SetCheckBoxCheckGlyphForegroundUncheckedPointerOver<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckGlyphForegroundUncheckedPointerOver", brush);
    }

    public static T SetCheckBoxBackgroundUncheckedPressed<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxBackgroundUncheckedPressed", brush);
    }

    public static T SetCheckBoxCheckBackgroundFillUncheckedPressed<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundFillUncheckedPressed", brush);
    }

    public static T SetCheckBoxForegroundUncheckedPressed<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxForegroundUncheckedPressed", brush);
    }

    public static T SetCheckBoxBorderBrushUncheckedPressed<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxBorderBrushUncheckedPressed", brush);
    }

    public static T SetCheckBoxCheckBackgroundStrokeUncheckedPressed<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundStrokeUncheckedPressed", brush);
    }

    public static T SetCheckBoxCheckGlyphForegroundUncheckedPressed<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckGlyphForegroundUncheckedPressed", brush);
    }

    public static T SetCheckBoxBackgroundUncheckedDisabled<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxBackgroundUncheckedDisabled", brush);
    }

    public static T SetCheckBoxCheckBackgroundFillUncheckedDisabled<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundFillUncheckedDisabled", brush);
    }

    public static T SetCheckBoxForegroundUncheckedDisabled<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxForegroundUncheckedDisabled", brush);
    }

    public static T SetCheckBoxBorderBrushUncheckedDisabled<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxBorderBrushUncheckedDisabled", brush);
    }

    public static T SetCheckBoxCheckBackgroundStrokeUncheckedDisabled<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckBackgroundStrokeUncheckedDisabled", brush);
    }

    public static T SetCheckBoxCheckGlyphForegroundUncheckedDisabled<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckGlyphForegroundUncheckedDisabled", brush);
    }

    public static T SetCheckBoxForegroundChecked<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxForegroundChecked", brush);
    }

    public static T SetCheckBoxCheckGlyphForegroundChecked<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckGlyphForegroundChecked", brush);
    }

    public static T SetCheckBoxForegroundCheckedPointerOver<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxForegroundCheckedPointerOver", brush);
    }

    public static T SetCheckBoxCheckGlyphForegroundCheckedPointerOver<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckGlyphForegroundCheckedPointerOver", brush);
    }

    public static T SetCheckBoxForegroundCheckedPressed<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxForegroundCheckedPressed", brush);
    }

    public static T SetCheckBoxCheckGlyphForegroundCheckedPressed<T>(this T element, Brush brush) where T : CheckBox
    {
        return SetResources(element, "CheckBoxCheckGlyphForegroundCheckedPressed", brush);
    }

    #endregion

    public static T SetOverlayCornerRadius<T>(this T element, CornerRadius radius) where T : FrameworkElement
    {
        return SetResources(element, "OverlayCornerRadius", radius);
    }

    public static T SetResources<T>(this T element, string name, object value) where T : FrameworkElement
	{
		if (element.Resources.Contains(name))
			element.Resources[name] = value;
		else
			element.Resources.Add(name, value);

		return element;
	}

	public static T AddItem<T>(this T element, object item) where T : ItemsControl
	{
		element.Items.Add(item);
		return element;
	}

	public static T InsertItem<T>(this T element, int index, object item) where T : ItemsControl
	{
		element.Items.Insert(index, item);
		return element;
	}

	public static T RemoveItem<T>(this T element, object item) where T : ItemsControl
	{
		element.Items.Remove(item);
		return element;
	}

	public static T RemoveItemAt<T>(this T element, int index) where T : ItemsControl
	{
		element.Items.RemoveAt(index);
		return element;
	}

	public static T ClearItems<T>(this T element) where T : ItemsControl
	{
		element.Items.Clear();
		return element;
	}

	public static T AddChildren<T>(this T element, UIElement item) where T : Panel
	{
		element.Children.Add(item);
		return element;
	}

	public static T InsertChildren<T>(this T element, int index, UIElement item) where T : Panel
	{
		element.Children.Insert(index, item);
		return element;
	}

	public static T RemoveChildren<T>(this T element, UIElement item) where T : Panel
	{
		element.Children.Remove(item);
		return element;
	}

	public static T RemoveChildrenAt<T>(this T element, int index) where T : Panel
	{
		element.Children.RemoveAt(index);
		return element;
	}

	public static T ClearChildrens<T>(this T element) where T : Panel
	{
		element.Children.Clear();
		return element;
	}

	public static T AddItem<T>(this T element, MenuItem item) where T : MenuItem
	{
		element.Items.Add(item);
		return element;
	}

	public static T AddSeparator<T>(this T element) where T : MenuItem
	{
		element.Items.Add(new Separator());
		return element;
	}

	public static Grid AddColumn(this Grid grid, double value, GridUnitType type = GridUnitType.Pixel, double minWidth = 0)
	{
		grid.ColumnDefinitions.Add(new ColumnDefinition { MinWidth = minWidth, Width = new GridLength(value, type) });
		return grid;
	}

	public static Grid AddRow(this Grid grid, double value, GridUnitType type = GridUnitType.Pixel, double minHeight = 0)
	{
		grid.RowDefinitions.Add(new RowDefinition { MinHeight = minHeight, Height = new GridLength(value, type) });
		return grid;
	}

	public static Grid AddElement(this Grid grid, UIElement element, int column = 0, int row = 0, int columnSpan = 1, int rowSpan = 1)
	{
		Grid.SetColumn(element, column);
		Grid.SetRow(element, row);
		Grid.SetColumnSpan(element, columnSpan);
		Grid.SetRowSpan(element, rowSpan);

		grid.Children.Add(element);
		return grid;
	}
}
