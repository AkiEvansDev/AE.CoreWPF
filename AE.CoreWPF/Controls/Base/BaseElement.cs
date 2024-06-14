using System.Windows;
using System.Windows.Controls;

namespace AE.CoreWPF.Controls;

public abstract class BaseElement : Grid
{
	public BaseElement(bool layout = true)
	{
		HorizontalAlignment = HorizontalAlignment.Stretch;
		VerticalAlignment = VerticalAlignment.Stretch;

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
		ColumnDefinitions.Add(new ColumnDefinition { MinWidth = minWidth, Width = new GridLength(value, type) });
	}

	protected void AddRow(double value, GridUnitType type = GridUnitType.Pixel, double minHeight = 0)
	{
		RowDefinitions.Add(new RowDefinition { MinHeight = minHeight, Height = new GridLength(value, type) });
	}

	protected GridLength GetColumnSize(int column)
	{
		return ColumnDefinitions[column].Width;
	}

	protected GridLength GetRowSize(int row)
	{
		return RowDefinitions[row].Height;
	}

	protected void AddElement(UIElement element, int column = 0, int row = 0, int columnSpan = 1, int rowSpan = 1)
	{
		SetColumn(element, column);
		SetRow(element, row);
		SetColumnSpan(element, columnSpan);
		SetRowSpan(element, rowSpan);

		Children.Add(element);
	}
}

public abstract class BaseDataElement<D> : BaseElement
{
	public D Data { get; protected set; }

	public BaseDataElement(D data) : base(false)
	{
		Data = data;

		Layout();
		Init();
	}
}

public abstract class BaseDataElement<D, P> : BaseElement
{
	public D Data { get; protected set; }
	protected P PrivateData { get; set; }

	public BaseDataElement(D data, P privateData) : base(false)
	{
		Data = data;
		PrivateData = privateData;

		Layout();
		Init();
	}
}
