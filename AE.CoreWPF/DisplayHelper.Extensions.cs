using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

	public static T SetMargin<T>(this T element, Thickness thickness) where T : FrameworkElement
	{
		element.Margin = thickness;
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

	public static T SetPadding<T>(this T element, Thickness thickness) where T : Control
	{
		element.Padding = thickness;
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

	public static T SetTextPadding<T>(this T element, Thickness thickness) where T : TextBlock
	{
		element.Padding = thickness;
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

	public static T SetBorder<T>(this T element, Thickness thickness) where T : Control
	{
		element.BorderThickness = thickness;
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
}
