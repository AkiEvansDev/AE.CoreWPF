using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AE.CoreWPF.Controls;

public sealed class Arc : Shape
{
	public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(nameof(Center), typeof(Point), typeof(Arc),
			new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsRender));

	public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(nameof(Angle), typeof(double), typeof(Arc),
			new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

	public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(double), typeof(Arc),
			new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));

	public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof(Radius), typeof(double), typeof(Arc),
			new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));

	static Arc()
	{
		DefaultStyleKeyProperty.OverrideMetadata(typeof(Arc), new FrameworkPropertyMetadata(typeof(Arc)));
	}

	public Point Center
	{
		get => (Point)GetValue(CenterProperty);
		set => SetValue(CenterProperty, value);
	}

	public double Angle
	{
		get => (double)GetValue(AngleProperty);
		set => SetValue(AngleProperty, value);
	}

	public double Progress
	{
		get => (double)GetValue(ProgressProperty);
		set => SetValue(ProgressProperty, value);
	}

	public double Radius
	{
		get => (double)GetValue(RadiusProperty);
		set => SetValue(RadiusProperty, value);
	}

	protected override Geometry DefiningGeometry
	{
		get
		{
			var progress = NormalizeProgress(Progress);

			if (progress == 100)
				return new EllipseGeometry(Center, Radius, Radius);

			var startAngle = Angle * (Math.PI / 180.0);
			var endAngle = (Angle + 360.0 * progress / 100.0) * (Math.PI / 180.0);

			var p0 = new Point(
				Center.X + Radius * Math.Cos(startAngle),
				Center.Y - Radius * Math.Sin(startAngle)
			);

			var p1 = new Point(
				Center.X + Radius * Math.Cos(endAngle),
				Center.Y - Radius * Math.Sin(endAngle)
			);

			var segments = new List<PathSegment>
			{
				new ArcSegment(p0, new Size(Radius, Radius), 0.0, Progress > 50, SweepDirection.Clockwise, true)
			};

			var figures = new List<PathFigure>
			{
				new(p1, segments, false)
				{
					IsClosed = false
				}
			};

			return new PathGeometry(figures, FillRule.EvenOdd, null);
		}
	}

	private static double NormalizeProgress(double progress)
	{
		return Math.Max(0, Math.Min(100, progress));
	}
}
