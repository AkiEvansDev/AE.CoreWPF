using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AE.CoreWPF.Controls;

public class ExpandButton : Button
{
	private Polygon polygon;

	private bool isExpanded;
	public bool IsExpanded
	{
		get => isExpanded;
		set
		{
			isExpanded = value;
			RefreshState();
		}
	}

	public ExpandButton()
	{
		Layout();
		RefreshState();
	}

	private void Layout()
	{
		Style = (Style)Application.Current.Resources["DefaultButtonStyle"];

		polygon = new Polygon
		{
			Width = 6,
			Height = 6,
			Stroke = DisplayHelper.Settings.ForegroundBrush,
			StrokeThickness = 1,
			RenderTransform = new RotateTransform(0, 3, 3),
		};

		polygon.Points.Add(new Point(6, 0));
		polygon.Points.Add(new Point(6, 6));
		polygon.Points.Add(new Point(0, 6));

		Content = polygon;
	}

	private void RefreshState()
	{
		if (IsExpanded)
		{
			(polygon.RenderTransform as RotateTransform).Angle = 0;
			polygon.Fill = polygon.Stroke;
		}
		else
		{
			(polygon.RenderTransform as RotateTransform).Angle = -45;
			polygon.Fill = DisplayHelper.Settings.TransperentBrush;
		}
	}
}
