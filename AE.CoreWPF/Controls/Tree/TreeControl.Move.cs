using System.Linq;
using System.Windows.Input;

namespace AE.CoreWPF.Controls;

public partial class TreeControl
{
	private TreeElement moveItem;

    internal bool StartMove(TreeElement item)
	{
		if (item.ParentTitle != null)
		{
			moveItem = item;
			MouseMove += OnScriptsTreeMouseMove;
			MouseUp += OnScriptsTreeMouseUp;
		}

		return IsMove();
	}

    internal bool IsMove()
	{
		return moveItem != null;
	}

    internal bool CanMove(TreeElement target)
	{
		if (moveItem == null)
			return false;

		if (moveItem.TreeControl.Id != target.TreeControl.Id)
			return false;

		if (target is TreeFolder targetFolder)
		{
			if (moveItem.Title == target.Title && moveItem.ParentTitle == target.ParentTitle)
				return false;

			if (targetFolder.Items.Any(i => i.Title == moveItem.Title))
				return false;

			if (target.ContainsParent(moveItem.Title))
				return false;

			return true;
		}

		return false;
	}

    internal void EndMove(TreeElement target)
	{
		if (moveItem == null)
			return;

		if (target is TreeFolder targetFolder && MoveValidate(moveItem.Title, moveItem.ParentTitle, target.Title))
		{
			targetFolder.Expand();
			moveItem.Move(targetFolder);
		}

		CancelMove();
	}

    internal void CancelMove()
	{
		MouseMove -= OnScriptsTreeMouseMove;
		MouseUp -= OnScriptsTreeMouseUp;
		moveItem = null;
	}

	private void OnScriptsTreeMouseMove(object sender, MouseEventArgs e)
	{
		if (moveItem != null && e.LeftButton == MouseButtonState.Pressed)
		{
			e.Handled = true;

			var end = e.GetPosition(this);

			if (end.X <= 6 || end.Y <= 6 || end.X >= ActualWidth - 6 || end.Y >= ActualHeight - 6)
				moveItem.MoveCaptureMouse();
			else
				moveItem.MoveReleaseMouseCapture();
		}
	}

	private void OnScriptsTreeMouseUp(object sender, MouseButtonEventArgs e)
	{
		moveItem?.MoveMouseUp(e);
	}

	protected virtual bool MoveValidate(string item, string from, string to)
	{
		return true;
	}
}
