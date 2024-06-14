using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using AE.CoreWPF.Controls;

using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;

namespace AE.CoreWPF.Dialogs;

public partial class FileDialog : DialogWindow
{
	public const int DefaultPanelWidth = 150;
	public const int PreviewItemsCount = 5;

	protected readonly Stack<string> Undo = new();
	protected readonly Stack<string> Redo = new();

	public string OpenPath { get; private set; }

	public bool CanSelectFolder { get; private set; }
	public bool CanSelectFile { get; private set; }

	public string[] ExtensionFilters { get; private set; }

	private FileDialogItemType type;

	private ScrollViewerEx defaultScroll;
	private ScrollViewerEx scroll;

	private SimpleStackPanel defaultPanel;
	private SimpleStackPanel panel;
	private FlexItemsControl previewPanel;

	private Button back;
	private Button forward;

	private TextBox navigatePath;
	private TextBox inputPath;

	private TextIconButton select;

	internal IEnumerable<FileDialogItem> DefaultItems => defaultPanel?.Children?.OfType<FileDialogItem>();
	internal IEnumerable<FileDialogItem> Items => type == FileDialogItemType.Line
		? panel?.Children?.OfType<FileDialogItem>()
		: previewPanel?.Items?.OfType<FileDialogItem>();

	private string result = null;

	private FileDialog(string basePath, bool canSelectFolder, bool canSelectFile, params string[] extensionFilters)
	{
		OpenPath = KnownFolders[2];

		CanSelectFolder = canSelectFolder;
		CanSelectFile = canSelectFile;
		ExtensionFilters = extensionFilters;

		Layout();
		LoadDefault();
		LoadPath(basePath, true, ignoreHistory: true);

		back.IsEnabled = false;
		forward.IsEnabled = false;
	}

	private void Layout()
	{
		Width = DefaultPanelWidth + DisplayHelper.Settings.ControlSizeCompact * FileDialogItem.PreviewScale * PreviewItemsCount + DisplayHelper.Settings.Space * (PreviewItemsCount + 2);
		MinWidth = 600;
		Height = 600;
		MinHeight = 400;

		var grid = new Grid();

		grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(DisplayHelper.Settings.ControlSizeCompact2 + DisplayHelper.Settings.MaxSpace, GridUnitType.Pixel) });
		grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
		grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(DisplayHelper.Settings.ControlSizeCompact2 + DisplayHelper.Settings.MaxSpace, GridUnitType.Pixel) });
		grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(DisplayHelper.Settings.ControlSizeCompact2 + DisplayHelper.Settings.MaxSpace, GridUnitType.Pixel) });

		grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(DefaultPanelWidth, GridUnitType.Pixel) });
		grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

		var topBorder = new Border
		{
			BorderBrush = DisplayHelper.Settings.StrokeBrush,
			Background = DisplayHelper.Settings.TertiaryBackgroundBrush,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			Margin = new Thickness(0),
			Padding = new Thickness(0),
			BorderThickness = new Thickness(0),
		};

		var topActionPanel = new Grid
		{
			Margin = new Thickness(DisplayHelper.Settings.MaxBorder, 0, DisplayHelper.Settings.MaxBorder, 0),
		};

		topActionPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
		topActionPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
		topActionPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
		topActionPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

		back = DisplayHelper.CreateButtonVariant(Symbol.Back, DisplayHelper.Settings.ControlSizeCompact1);
		forward = DisplayHelper.CreateButtonVariant(Symbol.Forward, DisplayHelper.Settings.ControlSizeCompact1);
		var refresh = DisplayHelper.CreateButtonVariant(Symbol.Refresh, DisplayHelper.Settings.ControlSizeCompact1);

		back.Click += (s, e) =>
		{
			if (Undo.TryPop(out var path))
			{
				Redo.Push(OpenPath);
				LoadPath(path, true, ignoreHistory: true, clearRedo: false);

				back.IsEnabled = Undo.Count > 0;
				forward.IsEnabled = true;
			}
		};
		forward.Click += (s, e) =>
		{
			if (Redo.TryPop(out var path))
			{
				Undo.Push(OpenPath);
				LoadPath(path, true, ignoreHistory: true, clearRedo: false);

				back.IsEnabled = true;
				forward.IsEnabled = Redo.Count > 0;
			}
		};
		refresh.Click += (s, e) => LoadPath(OpenPath, true, ignoreHistory: true, clearRedo: false);

		Grid.SetColumn(forward, 1);
		Grid.SetColumn(refresh, 3);

		topActionPanel.Children.Add(back);
		topActionPanel.Children.Add(forward);
		topActionPanel.Children.Add(refresh);

		navigatePath = DisplayHelper.CreateTextBoxVariant(DisplayHelper.Settings.ControlSizeCompact2, fontSize: DisplayHelper.Settings.FontSizeCompact, pattern: DisplayHelper.Settings.PathPattern)
			.SetMargin(0, DisplayHelper.Settings.Space, DisplayHelper.Settings.Space, DisplayHelper.Settings.Space);

		defaultScroll = DisplayHelper.CreateScroll(ScrollBarVisibility.Auto)
			.SetBorder(0, 0, DisplayHelper.Settings.Border, 0);

		defaultPanel = new SimpleStackPanel
		{
			Orientation = Orientation.Vertical,
			Margin = new Thickness(0, DisplayHelper.Settings.Space, 0, DisplayHelper.Settings.MaxSpace),
		};

		defaultScroll.Content = defaultPanel;

		scroll = DisplayHelper.CreateScroll(ScrollBarVisibility.Auto);

		panel = new SimpleStackPanel
		{
			Orientation = Orientation.Vertical,
			Margin = new Thickness(0, DisplayHelper.Settings.Space, 0, DisplayHelper.Settings.MaxSpace),
			Focusable = true,
			FocusVisualStyle = null,
		};

		previewPanel = new FlexItemsControl
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			HorizontalItemsAlignment = HorizontalItemsAlignment.CenterLeft,
			Margin = new Thickness(DisplayHelper.Settings.Space),
		};

		var bottomBorder = new Border
		{
			BorderBrush = DisplayHelper.Settings.StrokeBrush,
			Background = DisplayHelper.Settings.TertiaryBackgroundBrush,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			Margin = new Thickness(0),
			Padding = new Thickness(0),
			BorderThickness = new Thickness(0),
		};

		inputPath = DisplayHelper.CreateTextBoxVariant(DisplayHelper.Settings.ControlSizeCompact2, fontSize: DisplayHelper.Settings.FontSizeCompact, pattern: DisplayHelper.Settings.PathPattern)
			.SetMargin(0, DisplayHelper.Settings.Space, DisplayHelper.Settings.MaxSpace, DisplayHelper.Settings.Space);

		inputPath.IsReadOnly = true;

		var actionPanel = new SimpleStackPanel
		{
			Spacing = DisplayHelper.Settings.MaxSpace,
			Orientation = Orientation.Horizontal,
			HorizontalAlignment = HorizontalAlignment.Right,
			Margin = new Thickness(0, -DisplayHelper.Settings.Space, DisplayHelper.Settings.MaxSpace, 0),
		};

		select = DisplayHelper.CreateTextIconButtonVariant(text: "&=file_dialog_select", size: DisplayHelper.Settings.ControlSize);
		var cancel = DisplayHelper.CreateTextIconButtonVariant(text: "&=dialog_cancel", size: DisplayHelper.Settings.ControlSize);

		select.Click += (s, e) =>
		{
			result = string.IsNullOrEmpty(inputPath.Text) ? null : inputPath.Text;
			Close();
		};
		cancel.Click += (s, e) => Close();

		actionPanel.Children.Add(select);
		actionPanel.Children.Add(cancel);

		Grid.SetRow(defaultScroll, 1);
		Grid.SetRow(scroll, 1);
		Grid.SetRow(bottomBorder, 2);
		Grid.SetRow(inputPath, 2);
		Grid.SetRow(actionPanel, 3);

		Grid.SetColumn(navigatePath, 1);
		Grid.SetColumn(scroll, 1);
		Grid.SetColumn(inputPath, 1);
		Grid.SetColumn(actionPanel, 1);

		Grid.SetRowSpan(bottomBorder, 2);

		Grid.SetColumnSpan(topBorder, 2);
		Grid.SetColumnSpan(bottomBorder, 2);

		grid.Children.Add(topBorder);
		grid.Children.Add(topActionPanel);
		grid.Children.Add(navigatePath);
		grid.Children.Add(defaultScroll);
		grid.Children.Add(scroll);
		grid.Children.Add(bottomBorder);
		grid.Children.Add(inputPath);
		grid.Children.Add(actionPanel);

		Content = grid;

		navigatePath.LostFocus += OnNavigateLostFocus;
		navigatePath.KeyDown += OnNavigateKeyDown;

		panel.KeyDown += OnPanelKeyDown;
		previewPanel.KeyDown += OnPanelKeyDown;

		MouseDown += OnMouseDown;

		panel.Focus();
	}

	private void LoadDefault()
	{
		foreach (var path in KnownFolders)
			defaultPanel.Children.Add(new FileDialogItem(FileDialogItemType.Line, path, true, OnDefaultSelect));

		defaultPanel.Children.Add(new Border
		{
			Background = DisplayHelper.Settings.StrokeBrush,
			Height = DisplayHelper.Settings.MinBorder,
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Center,
			Margin = new Thickness(DisplayHelper.Settings.Space, DisplayHelper.Settings.MinSpace, DisplayHelper.Settings.Space, DisplayHelper.Settings.MinSpace),
			BorderThickness = new Thickness(0),
		});

		foreach (var drive in DriveInfo.GetDrives())
			defaultPanel.Children.Add(new FileDialogItem(drive, OnDefaultSelect));
	}

	private void LoadPath(string path, bool force = false, bool ignoreHistory = false, bool clearRedo = true)
	{
		if (File.Exists(path))
			path = Path.GetDirectoryName(path);

		if (!Directory.Exists(path))
			path = navigatePath.Text = OpenPath;

		if (!force && path == OpenPath)
		{
			SourceFocus();
			return;
		}

		if (!ignoreHistory)
		{
			Undo.Push(OpenPath);
			back.IsEnabled = true;
		}

		if (clearRedo)
		{
			Redo.Clear();
			forward.IsEnabled = false;
		}

		if (KnownFolders.Contains(path))
		{
			foreach (var item in DefaultItems)
				item.IsSelected = item.FullName == path;
		}

		OpenPath = navigatePath.Text = path;
		inputPath.Text = CanSelectFolder ? path : "";
		select.IsEnabled = CanSelectFolder;

		panel.Children.Clear();
		previewPanel.ItemsSource = null;

		type = FileDialogItemType.Line;
		IEnumerable<string> files = null;

		if (CanSelectFile)
		{
			files = Directory.GetFiles(path).Where(f => (File.GetAttributes(f) & FileAttributes.Hidden) == 0);

			if (ExtensionFilters != null && ExtensionFilters.Length > 0)
				files = files.Where(f => ExtensionFilters.Any(ef => f.EndsWith(ef, StringComparison.OrdinalIgnoreCase)));

			var imgCount = files.Count(f => ImageExtensions.Any(ef => f.EndsWith(ef, StringComparison.OrdinalIgnoreCase)));

			if (imgCount > 0 && imgCount >= files.Count() / 2)
				type = FileDialogItemType.Peview;
		}

		var items = Directory
			.GetDirectories(path)
			.OrderBy(p => Path.GetFileName(p))
			.Select(p => new FileDialogItem(type, p, true, OnSelect, OnOpen));

		if (!items.Any())
			items = Array.Empty<FileDialogItem>();

		if (CanSelectFile)
		{
			items = items.Concat(files
				.OrderBy(p => Path.GetFileName(p))
				.Select(p => new FileDialogItem(type, p, false, OnSelect))
			).ToList();
		}

		if (type == FileDialogItemType.Line)
		{
			foreach (var item in items)
				panel.Children.Add(item);
		}
		else
		{
			previewPanel.ItemsSource = items;
		}

		scroll.Content = type == FileDialogItemType.Line ? panel : previewPanel;
		scroll.HorizontalScrollBarVisibility = type == FileDialogItemType.Line ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled;

		SourceFocus();
		scroll.ScrollToVerticalOffset(0);

		(scroll.Content as FrameworkElement).Loaded += OnContentLoaded;
	}

	private void OnContentLoaded(object sender, RoutedEventArgs e)
	{
		(scroll.Content as FrameworkElement).Loaded -= OnContentLoaded;

		SourceFocus();
		scroll.ScrollToVerticalOffset(0);
	}

	private void OnDefaultSelect(FileDialogItem selectItem)
	{
		foreach (var item in DefaultItems)
			item.IsSelected = false;

		selectItem.IsSelected = true;
		OnOpen(selectItem);
	}

	private void OnSelect(FileDialogItem selectItem)
	{
		if (selectItem.IsSelected)
		{
			SourceFocus();
			return;
		}

		if ((selectItem.IsFolder && !CanSelectFolder) || (selectItem.IsFile && !CanSelectFile))
		{
			if (CanSelectFolder)
				inputPath.Text = OpenPath;

			select.IsEnabled = !string.IsNullOrEmpty(inputPath.Text);
		}
		else
		{
			inputPath.Text = selectItem.FullName;
			select.IsEnabled = true;
		}

		foreach (var item in Items)
		{
			item.IsSelected = false;
			item.ViewSelected = item.FullName == inputPath.Text;
		}

		selectItem.IsSelected = true;
		ScrollTo(selectItem);
	}

	private void OnOpen(FileDialogItem openItem)
	{
		LoadPath(openItem.FullName);
	}

	private void OnNavigateLostFocus(object sender, RoutedEventArgs e)
	{
		Navigate();
	}

	private void OnNavigateKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Escape)
		{
			navigatePath.Text = OpenPath;
			SourceFocus();
		}
		else if (e.Key == Key.Enter)
			Navigate();
	}

	private void Navigate()
	{
		Keyboard.ClearFocus();
		LoadPath(navigatePath.Text);
	}

	private void OnPanelKeyDown(object sender, KeyEventArgs e)
	{
		var count = Items.Count();

		if (count == 0)
			return;

		if (e.Key != Key.Down && e.Key != Key.Up && e.Key != Key.Left && e.Key != Key.Right)
			return;

		var item = Items.FirstOrDefault(i => i.IsSelected);

		if (item == null)
		{
			OnSelect(Items.First());
		}
		else
		{
			var index = Items.TakeWhile(i => !i.IsSelected).Count();

			if (type == FileDialogItemType.Peview)
			{
				if (e.Key == Key.Left)
					index--;
				else if (e.Key == Key.Right)
					index++;
				else
				{
					item = Items.ElementAt(index);
					var itemsCount = Items.GroupBy(i => i.Margin.Top, i => i).GroupBy(g => g.Count(), g => g.Key).Max(g => g.Key);

					if (e.Key == Key.Up)
						index -= itemsCount;
					else if (e.Key == Key.Down)
						index += itemsCount;

					if (index < 0 || index > count - 1)
						return;
				}
			}
			else
			{
				if (e.Key == Key.Up)
					index--;
				else if (e.Key == Key.Down)
					index++;
			}

			if (index < 0)
				index = count - 1;
			else if (index > count - 1)
				index = 0;

			OnSelect(Items.ElementAt(index));
		}

		e.Handled = true;
	}

	private void ScrollTo(FileDialogItem item)
	{
		double scrollTo;

		if (type == FileDialogItemType.Line)
		{
			var index = Items.ToList().IndexOf(item);
			scrollTo = index * item.Height;
		}
		else
		{
			scrollTo = item.Margin.Top;
		}

		if (scrollTo < scroll.VerticalOffset || scrollTo > scroll.VerticalOffset + scroll.ActualHeight - item.ActualHeight)
			scroll.ScrollToVerticalOffset(scrollTo);

		SourceFocus();
	}

	private void OnMouseDown(object sender, MouseButtonEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
			SourceFocus();
	}

	private void SourceFocus()
	{
		Keyboard.ClearFocus();
		(scroll.Content as UIElement).Focus();
	}
}
