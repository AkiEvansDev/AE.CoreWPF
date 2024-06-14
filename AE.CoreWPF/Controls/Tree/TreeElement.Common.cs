using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using ModernWpf.Controls;

namespace AE.CoreWPF.Controls;

public delegate void TreeElementSelected(TreeElement item);

public partial class TreeElement : BaseElement, IComparable<TreeElement>
{
    private static int CurrentId = 0;
    internal static int GetId()
    {
        CurrentId += 1;
        return CurrentId;
    }
    internal int Id { get; private set; }

    public TreeControl TreeControl { get; protected internal set; }
    public TreeFolder TreeParent { get; protected internal set; }

    private int level;
	protected internal int Level
	{
		get => level;
		set
		{
			level = value;
			UpdateLevel(value);
		}
	}

	public bool CanRename { get; private set; }
	public bool CanMove { get; private set; }

	public string Title
	{
		get => textBlock.Text;
		set
		{
			textBlock.Text = DisplayHelper.GetText(value);
			TreeParent?.Reorder(this);
		}
	}

	public string ParentTitle => TreeParent?.Title;

	private bool isSelected;
	public bool IsSelected
	{
		get => isSelected;
		set
		{
			if (value == IsSelected)
				return;

			isSelected = value;

			if (value)
			{
				border.Opacity = 1;
				TreeControl.OnSelect(this);

				TreeParent?.Expand();
			}
			else
				border.Opacity = 0;
		}
	}

	public new ContextMenu ContextMenu
	{
		get => border.ContextMenu;
		set => border.ContextMenu = value;
	}

	protected internal Border border;
	protected internal Grid namePanel;
	protected internal TextBlock textBlock;
	protected internal TextBox textBox;

	public TreeElement(bool canRename, bool canMove, Symbol symbol, Brush symbolColor)
	{
		Id = GetId();

		CanRename = canRename;
		CanMove = canMove;

		Layout(symbol, symbolColor);
	}

	protected override void Layout()
	{
		HorizontalAlignment = HorizontalAlignment.Stretch;

		AddColumn(1, GridUnitType.Star);
		AddRow(DisplayHelper.Settings.ControlSizeCompact + DisplayHelper.Settings.Border, GridUnitType.Pixel);

		border = new Border
		{
			Background = DisplayHelper.Settings.SelectBackgroundBrush,
			BorderBrush = DisplayHelper.Settings.StrokeBrush,
			BorderThickness = new Thickness(0, DisplayHelper.Settings.MinBorder, 0, DisplayHelper.Settings.MinBorder),
			Opacity = 0,
		};

		AddElement(border);

		border.MouseDown += OnMouseDown;
		border.MouseEnter += OnMouseEnter;
		border.MouseLeave += OnMouseLeave;
		border.MouseUp += OnMouseUp;
	}

	protected Grid GetNamePanel(Symbol symbol, Brush color)
	{
		namePanel = new Grid
		{
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Center,
			Margin = new Thickness(DisplayHelper.Settings.Space + DisplayHelper.Settings.ControlSizeCompact * Level, DisplayHelper.Settings.MinBorder, 0, DisplayHelper.Settings.MinBorder),
			IsHitTestVisible = false,
		};

		namePanel
			.AddColumn(1, GridUnitType.Auto)
			.AddColumn(DisplayHelper.Settings.Space, GridUnitType.Pixel)
			.AddColumn(1, GridUnitType.Star);

		var icon = new SymbolIcon(symbol)
		{
			Foreground = color,
			Margin = new Thickness(0, DisplayHelper.Settings.MinBorder, 0, 0),
			VerticalAlignment = VerticalAlignment.Stretch,
			LayoutTransform = new ScaleTransform(DisplayHelper.Settings.ControlScaleCompact, DisplayHelper.Settings.ControlScaleCompact),
		};

		textBlock = DisplayHelper.CreateTextBlock(fontSize: DisplayHelper.Settings.FontSizeCompact)
			.SetTextPadding(0, 0, DisplayHelper.Settings.MinSpace, 0);

		namePanel
			.AddElement(icon)
			.AddElement(textBlock, 2);

		if (CanRename)
		{
			textBox = DisplayHelper.CreateTextBox(DisplayHelper.Settings.ControlSizeCompact2, fontSize: DisplayHelper.Settings.FontSizeCompact, pattern: DisplayHelper.Settings.NamePattern)
				.SetPadding(0, 0, DisplayHelper.Settings.MinSpace, 0)
				.SetVisibility(Visibility.Collapsed);

			namePanel.AddElement(textBox, 2);

			textBox.LostFocus += OnTextBoxLostFocus;
			textBox.KeyDown += OnTextBoxKeyDown;
		}

		return namePanel;
	}

	protected virtual void Layout(Symbol symbol, Brush color)
	{
		Height = DisplayHelper.Settings.ControlSizeCompact + DisplayHelper.Settings.Border;
		AddElement(GetNamePanel(symbol, color));
	}

	public void Rename()
	{
		textBox.Text = textBlock.Text;
		textBlock.Visibility = Visibility.Collapsed;
		textBox.Visibility = Visibility.Visible;

		textBox.CaretIndex = textBox.Text.Length;
		textBox.SelectAll();

		textBox.Focus();
	}

	private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
	{
		OnEndRename();
	}

	private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Escape)
		{
			textBox.Text = textBlock.Text;
			OnEndRename();
		}
		else if (e.Key == Key.Enter)
			OnEndRename();
	}

	private void OnEndRename()
	{
		textBlock.Text = RenameValidate(textBlock.Text, textBox.Text);

		textBlock.Visibility = Visibility.Visible;
		textBox.Visibility = Visibility.Collapsed;

		IsSelected = true;
	}

	protected virtual string RenameValidate(string oldName, string newName)
	{
		return newName;
	}

	internal void Remove()
	{
		TreeParent.panel.RemoveChildren(this);
	}

	internal void Move(TreeFolder newParent)
	{
		Remove();
		newParent.AddItem(this);
	}

	protected virtual void UpdateLevel(int newLevel)
	{
		namePanel.Margin = new Thickness(
			DisplayHelper.Settings.Space + DisplayHelper.Settings.ControlSizeCompact * Level,
			namePanel.Margin.Top,
			namePanel.Margin.Right,
			namePanel.Margin.Bottom
		);
	}

	public bool ContainsParent(string name)
	{
		if (ParentTitle == name)
			return true;

		return TreeParent?.ContainsParent(name) == true;
	}

    public int CompareTo(TreeElement other)
    {
        return Title.CompareTo(other.Title);
    }
}

public class TreeFolder : TreeElement
{
	internal bool CanExpanded
	{
		get => panelBtn.Visibility == Visibility.Visible;
		set => panelBtn.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
	}

	public bool IsExpanded
	{
		get => panelBtn.IsExpanded;
		private set
		{
			panelBtn.IsExpanded = value;
			panel.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
		}
	}

	public IEnumerable<TreeElement> Items => panel.Children.OfType<TreeElement>();

	protected internal ExpandButton panelBtn;
	protected internal SimpleStackPanel panel;

	public TreeFolder(bool canRename, bool canMove, Symbol symbol, Brush symbolColor)
		: base(canRename, canMove, symbol, symbolColor) { }

	protected override void Layout(Symbol symbol, Brush color)
	{
		MinHeight = DisplayHelper.Settings.ControlSizeCompact + DisplayHelper.Settings.Border;

		AddRow(1, GridUnitType.Auto);

		var namePanel = GetNamePanel(symbol, color);

		panel = new SimpleStackPanel
		{
			Orientation = Orientation.Vertical,
			Visibility = Visibility.Collapsed,
		};

		panelBtn = DisplayHelper.CreateExpandButton()
			.SetHorizontalAlignment(HorizontalAlignment.Left)
			.SetMargin(namePanel.Margin.Left - DisplayHelper.Settings.ControlSizeCompact, 0, 0, 0);

		panelBtn.IsExpanded = false;

		panelBtn.Click += (s, e) =>
		{
			IsExpanded = !IsExpanded;
		};

		AddElement(panelBtn);
		AddElement(namePanel);
		AddElement(panel, row: 1);
	}

	public void AddItem(TreeElement item)
	{
		if (!IsExpanded)
			Expand();

		item.TreeControl = TreeControl;
        item.TreeParent = this;
        item.Level = Level + 1;

		InsertSorted(item);
	}

	internal void Reorder(TreeElement item)
	{
		panel.Children.Remove(item);
		InsertSorted(item);
	}

	internal void InsertSorted(TreeElement item)
	{
		var index = Items.Count(i => i.CompareTo(item) < 0);
		panel.Children.Insert(index, item);
	}

	public void Expand()
	{
		IsExpanded = true;
		TreeParent?.Expand();
	}

	protected override void UpdateLevel(int newLevel)
	{
		base.UpdateLevel(newLevel);

		if (panelBtn != null)
			panelBtn.Margin = new Thickness(namePanel.Margin.Left - DisplayHelper.Settings.ControlSizeCompact, 0, 0, 0);

		foreach (var item in Items)
			item.Level = newLevel + 1;
	}
}