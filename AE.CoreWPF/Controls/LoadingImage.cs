using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace AE.CoreWPF.Controls.Controls;

public class LoadingImage : Canvas
{
	public LoadingImage(double size)
	{
		Layout(size);
	}

	private void Layout(double size)
	{
		Background = DisplayHelper.Settings.TransperentBrush;
		Width = size;
		Height = size;
		IsHitTestVisible = false;

		var arc = new Arc
		{
			Stroke = DisplayHelper.Settings.ForegroundBrush,
			StrokeThickness = 4,
			Center = new Point(size / 2, size / 2),
			Angle = 0,
			Progress = 0,
			Radius = size,
		};

		var progressAnimate = new DoubleAnimation(0, 100, TimeSpan.FromSeconds(2))
		{
			AutoReverse = true,
			RepeatBehavior = RepeatBehavior.Forever,
		};

		var angleAnimate = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(1))
		{
			RepeatBehavior = RepeatBehavior.Forever,
		};

		Children.Add(arc);

		arc.BeginAnimation(Arc.ProgressProperty, progressAnimate);
		arc.BeginAnimation(Arc.AngleProperty, angleAnimate);
	}
}
