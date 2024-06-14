using System;
using System.Windows;
using System.Windows.Input;

namespace AE.CoreWPF.Controls;

public partial class TreeElement
{
	private Point? mouseDown;
	private void OnMouseDown(object sender, MouseButtonEventArgs e)
	{
		if (CanMove && e.LeftButton == MouseButtonState.Pressed && TreeControl.IsMove() == false)
		{
			mouseDown = e.GetPosition(this);
			border.MouseMove += OnMouseMove;
		}

		if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
			IsSelected = true;
	}

	private void OnMouseMove(object sender, MouseEventArgs e)
	{
		if (mouseDown != null && e.LeftButton == MouseButtonState.Pressed && (e.GetPosition(this) - mouseDown.Value).LengthSquared > 6 && TreeControl.StartMove(this))
		{
			border.MouseMove -= OnMouseMove;
			mouseDown = null;

			Mouse.OverrideCursor = Cursors.Hand;

			e.Handled = true;
		}
	}

	private void OnMouseEnter(object sender, MouseEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed && TreeControl.IsMove())
		{
			if (TreeControl.CanMove(this))
			{
				Mouse.OverrideCursor = Cursors.Hand;

				border.Background = DisplayHelper.Settings.Accent.WithAlpha(DisplayHelper.Settings.ColorOpacity2).ToBrush();
				border.Opacity = 1;
			}
			else
				Mouse.OverrideCursor = Cursors.No;

			e.Handled = true;
		}
	}

	private void OnMouseLeave(object sender, MouseEventArgs e)
	{
		mouseDown = null;

		if (e.LeftButton == MouseButtonState.Pressed && TreeControl.IsMove())
		{
			Mouse.OverrideCursor = null;

			border.Background = DisplayHelper.Settings.SelectBackgroundBrush;
			if (!IsSelected)
				border.Opacity = 0;

			e.Handled = true;
		}
	}

	internal void MoveCaptureMouse()
	{
		border.CaptureMouse();
	}

	internal void MoveReleaseMouseCapture()
	{
		border.ReleaseMouseCapture();
	}

	internal void MoveMouseUp(MouseButtonEventArgs e)
	{
		OnMouseUp(this, e);
	}

	private void OnMouseUp(object sender, MouseButtonEventArgs e)
	{
		mouseDown = null;

		if (e.ChangedButton == MouseButton.Left && TreeControl.IsMove())
		{
			Mouse.OverrideCursor = null;

			border.Background = DisplayHelper.Settings.SelectBackgroundBrush;
			border.Opacity = 0;

			if (TreeControl.CanMove(this))
                TreeControl.EndMove(this);
			else
                TreeControl.CancelMove();

			e.Handled = true;
		}
	}
}