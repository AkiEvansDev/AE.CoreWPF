using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using ModernWpf.Controls.Primitives;

namespace AE.CoreWPF.Controls;

public delegate void SelectedTabEventHandler<H, C>(TabControl<H, C> source, TabItem<H, C> tab)
	where H : TabHeader
	where C : FrameworkElement;

public delegate void ClosedTabEventHandler<H, C>(TabControl<H, C> source, IEnumerable<TabItem<H, C>> tabs)
	where H : TabHeader
	where C : FrameworkElement;

public delegate void CloseTabAction(string header, bool all, bool other);

public class TabItem<H, C> : TabItem
	where H : TabHeader
	where C : FrameworkElement
{
	public new H Header => base.Header as H;
	public new C Content => base.Content as C;

	public TabItem(H header, C content)
	{
		base.Header = header;
		base.Content = content;

		Style = Application.Current.FindResource(typeof(TabItem)) as Style;

		ControlHelper.SetCornerRadius(this, new CornerRadius(0));
	}
}

public class TabControl<H, C> : TabControl
	where H : TabHeader
	where C : FrameworkElement
{
	public event SelectedTabEventHandler<H, C> TabSelected;
	public event ClosedTabEventHandler<H, C> TabsClosed;

	public new IEnumerable<TabItem<H, C>> Items => base.Items.OfType<TabItem<H, C>>();

	public new C SelectedContent => SelectedItem?.Content;
	public new TabItem<H, C> SelectedItem
	{
		get => base.SelectedItem as TabItem<H, C>;
		set => base.SelectedItem = value;
	}

	public TabControl()
	{
		Style = Application.Current.FindResource(typeof(TabControl)) as Style;

		Background = DisplayHelper.Settings.TransperentBrush;
		Padding = new Thickness(0);
	}

	protected override void OnSelectionChanged(SelectionChangedEventArgs e)
	{
		base.OnSelectionChanged(e);

		foreach (var tab in Items)
		{
			tab.Header.SetSelect(false);

			if (tab.IsSelected)
			{
				tab.Header.SetSelect(true);
				TabSelected?.Invoke(this, tab);
			}
		}
	}

	public TabItem<H, C> AddTab(H header, C content)
	{
		var tab = new TabItem<H, C>(header, content);
		base.Items.Add(tab);

		return tab;
	}

	public TabItem<H, C> InsertTab(int index, H header, C content)
	{
		var tab = new TabItem<H, C>(header, content);
		base.Items.Insert(index, tab);

		return tab;
	}

	public void RemoveTab(TabItem<H, C> tab)
	{
		base.Items.Remove(tab);
	}

	public void RemoveTabAt(int index)
	{
		base.Items.RemoveAt(index);
	}

	public void ClearTabs()
	{
		base.Items.Clear();
	}

	protected void CloseTab(string header, bool all, bool other)
	{
		IEnumerable<TabItem<H, C>> tabs = null;

		if (all)
		{
			if (other)
				tabs = Items.Where(i => i.Header.Title != header).ToList();
			else
				tabs = Items.ToList();
		}
		else
			tabs = Items.Where(i => i.Header.Title == header).ToList();

		var isSelected = tabs.Any(t => t.IsSelected);

		foreach (var tab in tabs)
			base.Items.Remove(tab);

		if (isSelected)
			SelectedIndex = 0;

		TabsClosed?.Invoke(this, tabs);
	}
}

public class TabControl<C> : TabControl<TabHeader, C>
	where C : FrameworkElement
{
	public TabItem<TabHeader, C> AddTab(string header, C content, bool canClose = true)
	{
		return AddTab(new TabHeader(header, canClose ? CloseTab : null), content);
	}

	public TabItem<TabHeader, C> InsertTab(int index, string header, C content, bool canClose = true)
	{
		return InsertTab(index, new TabHeader(header, canClose ? CloseTab : null), content);
	}
}

public class VariantTabControl<C> : TabControl<VariantTabHeader, C>
	where C : FrameworkElement
{
	public VariantTabControl() : base()
	{
		TabStripPlacement = Dock.Bottom;

		TabControlHelper.SetTabStripFooter(this, new Border
		{
			BorderBrush = DisplayHelper.Settings.StrokeBrush,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Top,
			BorderThickness = new Thickness(0, 0, 0, DisplayHelper.Settings.MinBorder),
		});

		Resources.Add("TabViewItemHeaderBackground", DisplayHelper.Settings.SecondaryBackgroundBrush);
		Resources.Add("TabViewItemHeaderBackgroundSelected", DisplayHelper.Settings.SecondaryBackgroundBrush);
	}

	public TabItem<VariantTabHeader, C> AddTab(string header, C content, bool canClose = false)
	{
		return AddTab(new VariantTabHeader(header, canClose ? CloseTab : null), content);
	}

	public TabItem<VariantTabHeader, C> InsertTab(int index, string header, C content, bool canClose = false)
	{
		return InsertTab(index, new VariantTabHeader(header, canClose ? CloseTab : null), content);
	}
}
