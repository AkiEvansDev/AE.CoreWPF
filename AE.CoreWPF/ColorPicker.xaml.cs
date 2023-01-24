using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

using ModernWpf.Controls;

using DColor = System.Drawing.Color;

namespace AE.CoreWPF;

public partial class ColorPicker : UserControl
{
    private const double hH = 255; 
    private const double svAddH = 55;
    private const double svH = 200;
    private const double svV = 100;

    public delegate void ColorChangedDelegate(ColorPicker sender, Color color);

    public event ColorChangedDelegate SelectColorChanged;

    private static readonly DependencyProperty HColorProperty =
        DependencyProperty.Register(nameof(HColor), typeof(Color), typeof(ColorPicker), new PropertyMetadata(Color.FromArgb(255, 255, 0, 0)));

    public static readonly DependencyProperty SelectColorProperty =
        DependencyProperty.Register(nameof(SelectColor), typeof(Color), typeof(ColorPicker), new PropertyMetadata(Color.FromArgb(255, 255, 0, 0), OnSelectColorChanged));

    private Color HColor
    {
        get => (Color)GetValue(HColorProperty);
        set => SetValue(HColorProperty, value);
    }

    public Color SelectColor
    {
        get => (Color)GetValue(SelectColorProperty);
        set => SetValue(SelectColorProperty, value);
    }

    private static void OnSelectColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is ColorPicker colorPicker)
        {
            if (colorPicker.hPositionH == null && colorPicker.svPositionH == null && colorPicker.svPositionV == null)
            {
                ColorToHSV(
                    DColor.FromArgb(colorPicker.SelectColor.R, colorPicker.SelectColor.G, colorPicker.SelectColor.B),
                    out double h,
                    out double s,
                    out double v
                );

                Canvas.SetLeft(colorPicker.H, NormolizePosition(h * hH / 360.0, 0, hH));
                Canvas.SetLeft(colorPicker.SV, NormolizePosition(s * 100.0 * svH / 100.0 + svAddH, svAddH, svH + svAddH));
                Canvas.SetTop(colorPicker.SV, NormolizePosition((100 - v * 100.0) * svV / 100.0, 0, svV));

                colorPicker.HColor = ColorFromHSV(h, 1, 1);
            }

            colorPicker.R.Value = colorPicker.SelectColor.R;
            colorPicker.G.Value = colorPicker.SelectColor.G;
            colorPicker.B.Value = colorPicker.SelectColor.B;

            colorPicker.SelectColorChanged?.Invoke(colorPicker, colorPicker.SelectColor);
        }
    }

    public ColorPicker()
    {
        InitializeComponent();
        SelectColor = Color.FromRgb(255, 0, 0);
    }

    private void OnHMouseDown(object sender, MouseButtonEventArgs e)
    {
        HThumb.RaiseEvent(e);
    }

    public double? hPositionH = null;
    private void OnHThumbDragStarted(object sender, DragStartedEventArgs e)
    {
        hPositionH = e.HorizontalOffset - 15;

        Canvas.SetLeft(H, NormolizePosition(hPositionH.Value, 0, hH));
        UpdateColor();

        Keyboard.ClearFocus();
    }

    private void OnHThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
        if (hPositionH != null)
        {
            Canvas.SetLeft(H, NormolizePosition(hPositionH.Value + e.HorizontalChange, 0, hH));
            UpdateColor();
        }
    }

    private void OnHThumbDragCompleted(object sender, DragCompletedEventArgs e)
    {
        hPositionH = null;
    }

    private void OnSVMouseDown(object sender, MouseButtonEventArgs e)
    {
        SVThumb.RaiseEvent(e);
    }

    public double? svPositionH = null;
    public double? svPositionV = null;
    private void OnSVThumbDragStarted(object sender, DragStartedEventArgs e)
    {
        svPositionH = e.HorizontalOffset - 15;
        svPositionV = e.VerticalOffset - 15;

        Canvas.SetLeft(SV, NormolizePosition(svPositionH.Value, svAddH, svH + svAddH));
        Canvas.SetTop(SV, NormolizePosition(svPositionV.Value, 0, svV));
        UpdateColor();

        Keyboard.ClearFocus();
    }

    private void OnSVThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
        if (svPositionH != null && svPositionV != null)
        {
            Canvas.SetLeft(SV, NormolizePosition(svPositionH.Value + e.HorizontalChange, svAddH, svH + svAddH));
            Canvas.SetTop(SV, NormolizePosition(svPositionV.Value + e.VerticalChange, 0, svV));
            UpdateColor();
        }
    }

    private void OnSVThumbDragCompleted(object sender, DragCompletedEventArgs e)
    {
        svPositionV = null;
        svPositionH = null;
    }

    private void OnRValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        var value = (byte)args.NewValue;
        if (double.IsNaN(args.NewValue))
            sender.Value = value = 0;

        if (SelectColor.R != value)
            SelectColor = Color.FromRgb(value, SelectColor.G, SelectColor.B);
    }

    private void OnGValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        var value = (byte)args.NewValue;
        if (double.IsNaN(args.NewValue))
            sender.Value = value = 0;

        if (SelectColor.G != value)
            SelectColor = Color.FromRgb(SelectColor.R, value, SelectColor.B);
    }

    private void OnBValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        var value = (byte)args.NewValue;
        if (double.IsNaN(args.NewValue))
            sender.Value = value = 0;

        if (SelectColor.B != value)
            SelectColor = Color.FromRgb(SelectColor.R, SelectColor.G, value);
    }

    private void UpdateColor()
    {
        var h = Canvas.GetLeft(H) * 360.0 / hH;
        var s = (Canvas.GetLeft(SV) - svAddH) * 100.0 / svH / 100.0;
        var v = (100.0 - Canvas.GetTop(SV) * 100.0 / svV) / 100.0;

        HColor = ColorFromHSV(h, 1, 1);
        SelectColor = ColorFromHSV(h, s, v);
    }

    private static double NormolizePosition(double top, double min, double max)
    {
        if (top < min)
            top = min;

        if (top > max)
            top = max;

        return Math.Round(top);
    }

    private static void ColorToHSV(DColor color, out double hue, out double saturation, out double value)
    {
        int max = Math.Max(color.R, Math.Max(color.G, color.B));
        int min = Math.Min(color.R, Math.Min(color.G, color.B));

        hue = color.GetHue();
        saturation = (max == 0) ? 0 : 1d - (1d * min / max);
        value = max / 255d;
    }

    private static Color ColorFromHSV(double hue, double saturation, double value)
    {
        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        value *= 255;
        var v = Convert.ToByte(value);
        var p = Convert.ToByte(value * (1 - saturation));
        var q = Convert.ToByte(value * (1 - f * saturation));
        var t = Convert.ToByte(value * (1 - (1 - f) * saturation));

        if (hi == 0)
            return Color.FromArgb(255, v, t, p);
        else if (hi == 1)
            return Color.FromArgb(255, q, v, p);
        else if (hi == 2)
            return Color.FromArgb(255, p, v, t);
        else if (hi == 3)
            return Color.FromArgb(255, p, q, v);
        else if (hi == 4)
            return Color.FromArgb(255, t, p, v);
        else
            return Color.FromArgb(255, v, p, q);
    }
}
