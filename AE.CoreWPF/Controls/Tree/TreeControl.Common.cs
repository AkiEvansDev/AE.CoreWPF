using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using ModernWpf.Controls;

namespace AE.CoreWPF.Controls;

public delegate void TreeElementSelectedEventHandler(TreeControl control, TreeElement element);

public partial class TreeControl : Border
{
	public event TreeElementSelectedEventHandler ElementSelected;
   
	private static int CurrentId = 0;
    internal static int GetId()
    {
        CurrentId += 1;
        return CurrentId;
    }
    internal int Id { get; private set; }

	public string Title
	{
		get => title.Text;
		set
		{
			title.Text = DisplayHelper.GetText(value);
			title.Visibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
		}
	}

	public TreeFolder CurrentElement
	{
		get => scroll.Content as TreeFolder;
		set
		{
			foreach (var item in value.Items)
				item.Remove();

			value.TreeControl = this;
            value.TreeParent = null;
            value.Level = 0;

            value.CanExpanded = false;
			value.Expand();

			scroll.Content = value;
		}
	}

	private TextBlock title;
	private ScrollViewerEx scroll;

	public TreeControl()
	{
		Id = GetId();
		Layout();
	}

	protected virtual void Layout()
	{
		Background = DisplayHelper.Settings.TransperentBrush;
		BorderBrush = DisplayHelper.Settings.StrokeBrush;
		HorizontalAlignment = HorizontalAlignment.Stretch;
		VerticalAlignment = VerticalAlignment.Stretch;
		BorderThickness = new Thickness(0, DisplayHelper.Settings.Border, 0, 0);

		var panel = new SimpleStackPanel
		{
			Orientation = Orientation.Vertical,
		};

		title = DisplayHelper.CreateSecondaryTextBlock("", 14, FontWeights.Light)
			.SetTextPadding(DisplayHelper.Settings.Space, DisplayHelper.Settings.MinSpace)
			.SetVisibility(Visibility.Collapsed);

		scroll = DisplayHelper.CreateScroll(ScrollBarVisibility.Auto, ScrollBarVisibility.Auto);

		Child = panel
			.AddChildren(title)
			.AddChildren(scroll);
	}

	internal void OnSelect(TreeElement element)
	{
		SetSelect(element);
		ElementSelected?.Invoke(this, element);
	}

	public virtual void SetSelect(string name)
	{
		var item = GetElement(name);
		SetSelect(item);
	}

	public virtual TreeElement GetElement(string name)
	{
		return GetElements().FirstOrDefault(g => g.Title == name);
	}

	public void SetSelect(TreeElement element)
	{
		foreach (var i in GetElements())
			i.IsSelected = i.Id == element.Id;
	}

	public IEnumerable<TreeElement> GetElements()
	{
		yield return CurrentElement;

		foreach (var item in GetElements(CurrentElement.Items))
			yield return item;
	}

	public void AddElement(string parent, TreeElement element)
	{
		var target = CurrentElement.Title == parent
			? CurrentElement
			: GetElements(CurrentElement.Items).First(i => i.Title == parent) as TreeFolder;

		target.AddItem(element);
	}

	public void MoveElement(string title, string to)
	{
		var source = GetElements(CurrentElement.Items).First(i => i.Title == title);
		var target = CurrentElement.Title == to
			? CurrentElement
			: GetElements(CurrentElement.Items).First(i => i.Title == to) as TreeFolder;

		source.Move(target);
	}

	public TreeElement DeleteElement(string title)
	{
		var item = GetElements(CurrentElement.Items).First(i => i.Title == title);

		DeleteElement(item);
		return item;
	}

	public void DeleteElement(TreeElement item)
	{
		if (item.IsSelected)
			SetSelect(CurrentElement.Title);

		item.Remove();
	}

	private static IEnumerable<TreeElement> GetElements(IEnumerable<TreeElement> items)
	{
		foreach (var item in items)
		{
			yield return item;

			if (item is TreeFolder folder)
			{
				foreach (var subItem in GetElements(folder.Items))
					yield return subItem;
			}
		}
	}
}
