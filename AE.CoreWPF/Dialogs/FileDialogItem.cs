using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using AE.CoreWPF.Controls;
using AE.CoreWPF.Controls.Controls;

using ModernWpf.Controls;

using Image = System.Windows.Controls.Image;
using Size = System.Windows.Size;

namespace AE.CoreWPF.Dialogs;

public enum FileDialogItemType
{
	Line = 0,
	Peview = 1,
}

public class FileDialogItem : Grid
{
	public const int PreviewScale = 5;

	private readonly Action<FileDialogItem> onSelect;
	private readonly Action<FileDialogItem> onOpen;

	public bool IsFile { get; }
	public bool IsFolder { get; }
	public bool IsDrive { get; }
	public bool IsKnown { get; }

	public string DisplayName { get; }
	public string FullName { get; }

	public bool ViewSelected
	{
		get => border.Opacity == 1;
		set => border.Opacity = value ? 1 : 0;
	}

	private bool isSelected;
	public bool IsSelected
	{
		get => isSelected;
		set => isSelected = ViewSelected = value;
	}

	private Border border;

	private FileDialogItem(Action<FileDialogItem> select, Action<FileDialogItem> open)
	{
		onSelect = select;
		onOpen = open;
	}

	public FileDialogItem(DriveInfo drive, Action<FileDialogItem> select, Action<FileDialogItem> open = null) : this(select, open)
	{
		IsFile = false;
		IsFolder = false;
		IsDrive = true;
		IsKnown = false;

		FullName = drive.Name;
		DisplayName = $"({drive.Name.TrimEnd('\\')}) {drive.VolumeLabel}";

		Layout(FileDialogItemType.Line);
	}

	public FileDialogItem(FileDialogItemType type, string path, bool isFolder, Action<FileDialogItem> select, Action<FileDialogItem> open = null) : this(select, open)
	{
		IsFile = !isFolder;
		IsFolder = isFolder;
		IsDrive = false;
		IsKnown = isFolder && FileDialog.KnownFolders.Contains(path);

		FullName = path;
		DisplayName = Path.GetFileName(path);

		Layout(type);
	}

	private void Layout(FileDialogItemType type)
	{
		var scale = 1;

		if (type == FileDialogItemType.Line)
		{
			Height = DisplayHelper.Settings.ControlSizeCompact + DisplayHelper.Settings.MaxBorder;
			HorizontalAlignment = HorizontalAlignment.Stretch;

			ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star), MinWidth = DisplayHelper.Settings.ControlSize * 3 });
			RowDefinitions.Add(new RowDefinition { Height = new GridLength(Height, GridUnitType.Pixel) });
		}
		else
		{
			scale = PreviewScale;

			Width = DisplayHelper.Settings.ControlSizeCompact * scale;
			Height = Width + DisplayHelper.Settings.ControlSizeCompact + DisplayHelper.Settings.MaxBorder;

			RowDefinitions.Add(new RowDefinition { Height = new GridLength(Width, GridUnitType.Pixel) });
			RowDefinitions.Add(new RowDefinition { Height = new GridLength(Height - Width, GridUnitType.Pixel) });

			ToolTip = DisplayHelper.CreateToolTip(DisplayName, verticalOffset: -RowDefinitions[1].Height.Value);
		}

		border = new Border
		{
			Background = DisplayHelper.Settings.SelectBackgroundBrush,
			BorderBrush = DisplayHelper.Settings.StrokeBrush,
			BorderThickness = type == FileDialogItemType.Line
				? new Thickness(0, DisplayHelper.Settings.MinBorder, 0, DisplayHelper.Settings.MinBorder)
				: new Thickness(DisplayHelper.Settings.MinBorder),
			Opacity = 0,
		};

		border.MouseDown += OnMouseDown;

		Children.Add(border);

		var namePanel = type == FileDialogItemType.Line
			? new Grid
			{
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(DisplayHelper.Settings.MaxSpace, DisplayHelper.Settings.Border, 0, DisplayHelper.Settings.Border),
				IsHitTestVisible = false,
			}
			: this;

		if (type == FileDialogItemType.Line)
		{
			namePanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
			namePanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(DisplayHelper.Settings.Space, GridUnitType.Pixel) });
			namePanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
		}

		if (IsFolder && !IsKnown && !IsDrive)
		{
			var icon = new SymbolIcon(Symbol.Folder)
			{
				Foreground = DisplayHelper.Settings.WarningColorBrush,
				LayoutTransform = new ScaleTransform(DisplayHelper.Settings.ControlScaleCompact + (type == FileDialogItemType.Line ? 0 : 2), DisplayHelper.Settings.ControlScaleCompact + (type == FileDialogItemType.Line ? 0 : 2)),
				IsHitTestVisible = false,
			};

			namePanel.Children.Add(icon);
		}
		else
		{
			Image image = null;

			if (type == FileDialogItemType.Peview && FileDialog.ImageExtensions.Any(ef => FullName.EndsWith(ef, StringComparison.OrdinalIgnoreCase)))
			{
				var loading = new LoadingImage(DisplayHelper.Settings.ControlSizeCompact);

				image = new AsyncImage
				{
					Width = DisplayHelper.Settings.ControlSizeCompact * scale,
					Height = DisplayHelper.Settings.ControlSizeCompact * scale,
					DecodePixelWidth = (int)DisplayHelper.Settings.ControlSizeCompact * scale,
					ImagePath = FullName,
				};

				namePanel.Children.Add(loading);
				(image as AsyncImage).ImageLoad += (s) => namePanel.Children.Remove(loading);
			}
			else
			{
				image = new Image
				{
					Width = DisplayHelper.Settings.ControlSizeCompact * (type == FileDialogItemType.Line ? 1 : scale / 2.0),
					Height = DisplayHelper.Settings.ControlSizeCompact * (type == FileDialogItemType.Line ? 1 : scale / 2.0),
				};

				using var icon = IsKnown || IsDrive
					? WinFileHelper.ExtractIconFromPath(FullName)
					: Icon.ExtractAssociatedIcon(FullName);

				image.Source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			}

			image.LayoutTransform = new ScaleTransform(DisplayHelper.Settings.ControlScaleCompact, DisplayHelper.Settings.ControlScaleCompact);
			image.IsHitTestVisible = false;

			namePanel.Children.Add(image);
		}

		var textBlock = DisplayHelper.CreateTextBlock(fontSize: DisplayHelper.Settings.FontSizeCompact);
		textBlock.IsHitTestVisible = false;

		if (type == FileDialogItemType.Line)
			textBlock.Padding = new Thickness(0, 0, DisplayHelper.Settings.MaxSpace, 0);
		else
		{
			textBlock.TextAlignment = TextAlignment.Left;
			textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			textBlock.Margin = new Thickness(DisplayHelper.Settings.MaxSpace, 0, DisplayHelper.Settings.MaxSpace, 0);
			textBlock.ClipToBounds = true;

		}

		textBlock.Text = DisplayName;

		if (type == FileDialogItemType.Line)
		{
			SetColumn(textBlock, 2);
		}
		else
		{
			textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			textBlock.Arrange(new Rect(textBlock.DesiredSize));

			if (textBlock.ActualWidth < Width)
				ToolTip = null;

			SetRowSpan(border, 2);
			SetRow(textBlock, 1);
		}

		namePanel.Children.Add(textBlock);

		if (type == FileDialogItemType.Line)
			Children.Add(namePanel);
	}

	private void OnMouseDown(object sender, MouseButtonEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
		{
			if (e.ClickCount == 1)
				onSelect?.Invoke(this);
			else if (e.ClickCount == 2)
				onOpen?.Invoke(this);
		}
	}
}
