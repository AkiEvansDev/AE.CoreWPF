using System.Windows;
using System.Windows.Controls;

namespace AE.CoreWPF.Controls;

public abstract class BaseControl : Border
{
	private readonly Grid grid;

	public BaseControl(bool layout = true)
	{
		Child = grid = new Grid
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
		};

		Loaded += OnLoaded;

		if (layout)
		{
			Layout();
			Init();
		}
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		Loaded -= OnLoaded;

		FirstLoaded();
	}

	protected virtual void Layout() { }
	protected virtual void Init() { }
	protected virtual void FirstLoaded() { }

	protected void AddColumn(double value, GridUnitType type = GridUnitType.Pixel, double minWidth = 0)
	{
		grid.ColumnDefinitions.Add(new ColumnDefinition { MinWidth = minWidth, Width = new GridLength(value, type) });
	}

	protected void AddRow(double value, GridUnitType type = GridUnitType.Pixel, double minHeight = 0)
	{
		grid.RowDefinitions.Add(new RowDefinition { MinHeight = minHeight, Height = new GridLength(value, type) });
	}

	protected GridLength GetColumnSize(int column)
	{
		return grid.ColumnDefinitions[column].Width;
	}

	protected GridLength GetRowSize(int row)
	{
		return grid.RowDefinitions[row].Height;
	}

	protected void AddElement(UIElement element, int column = 0, int row = 0, int columnSpan = 1, int rowSpan = 1)
	{
		Grid.SetColumn(element, column);
		Grid.SetRow(element, row);
		Grid.SetColumnSpan(element, columnSpan);
		Grid.SetRowSpan(element, rowSpan);

		grid.Children.Add(element);
	}
}

public abstract class BaseDataControl<D> : BaseControl
{
	public D Data { get; }

	public BaseDataControl(D data) : base(false)
	{
		Data = data;

		Layout();
		Init();
	}
}

public abstract class BaseDataControl<D, P> : BaseControl
{
	public D Data { get; }
	protected P PrivateData { get; }

	public BaseDataControl(D data, P privateData) : base(false)
	{
		Data = data;
		PrivateData = privateData;

		Layout();
		Init();
	}
}
