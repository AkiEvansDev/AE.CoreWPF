using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AE.CoreWPF;

public class FlexItemsControl : ItemsControl
{
    public static readonly DependencyProperty SpacingProperty = 
        DependencyProperty.Register(nameof(Spacing), typeof(int), typeof(FlexItemsControl));

    public int Spacing
    {
        get => (int)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public FlexItemsControl()
    {
        ItemsPanel = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(Grid)));
    }

    public virtual void Update()
    {
        if (Items == null)
            return;

        var matrix = new List<int[]> { new[] { 0, (int)ActualWidth, 0 } };
        var hMax = 0;
        
        foreach (var item in Items)
        {
            var element = GetFrameworkItem(item);
            if (element != null)
            {
                var size = new[] { (int)element.ActualWidth + Spacing, (int)element.ActualHeight + Spacing };
                var point = GetAttachPoint(matrix, size[0]);

                matrix = UpdateAttachArea(matrix, point, size);
                hMax = Math.Max(hMax, point[1] + size[1]);

                UpdateAlignment(element);
                if (Math.Abs(element.Margin.Left - point[0]) > 1 || Math.Abs(element.Margin.Top - point[1]) > 1)
                    SetPosition(element, point[1], point[0]);
            }
        }
    }

    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        var newValueArray = newValue as object[] ?? newValue.Cast<object>().ToArray();
        base.OnItemsSourceChanged(oldValue, newValueArray);

        if (newValueArray != null && newValueArray.Any())
        {
            var element = GetFrameworkItem(newValueArray.Last());
            HandleUpdate(element);
        }
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);

        if (e.NewItems != null && e.NewItems.Count > 0)
        {
            var element = GetFrameworkItem(e.NewItems.Cast<object>().Last());
            HandleUpdate(element);
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
            Update();
    }

    protected override void OnChildDesiredSizeChanged(UIElement child)
    {
        base.OnChildDesiredSizeChanged(child);
        HandleChildDesiredSizeChanged(child);
    }

    protected virtual void HandleChildDesiredSizeChanged(UIElement child)
    {
        if (child is FrameworkElement frameworkElement)
            HandleUpdate(frameworkElement);
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        HandleRenderSizeChanged(sizeInfo);
    }

    protected virtual void HandleRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        Update();
    }

    protected virtual void HandleUpdate(FrameworkElement element)
    {
        if (element != null)
        {
            if (element.IsLoaded)
                Update();
            else
                element.Loaded += (s, e) => Update();
        }
    }

    protected virtual void SetPosition(FrameworkElement element, int newTop, int newLeft)
    {
        if (element != null)
            element.Margin = new Thickness(newLeft, newTop, 0, 0);
    }

    protected virtual void UpdateAlignment(FrameworkElement element)
    {
        if (element != null)
        {
            element.HorizontalAlignment = HorizontalAlignment.Left;
            element.VerticalAlignment = VerticalAlignment.Top;
        }
    }

    protected virtual FrameworkElement GetFrameworkItem(object item)
    {
        if (item is FrameworkElement frameworkElement1)
            return frameworkElement1;

        if (ItemContainerGenerator.ContainerFromItem(item) is FrameworkElement frameworkElement2)
            return frameworkElement2;

        return null;
    }

    protected static int[] GetAttachPoint(List<int[]> mtx, int width)
    {
        mtx.Sort(MatrixSortDepth);
        var max = mtx[^1][2];

        for (int i = 0, length = mtx.Count; i < length; i++)
        {
            if (mtx[i][2] >= max)
                break;

            if (mtx[i][1] - mtx[i][0] >= width)
                return new[] { mtx[i][0], mtx[i][2] };
        }

        return new[] { 0, max };
    }

    protected static List<int[]> MatrixJoin(List<int[]> mtx, int[] cell)
    {
        mtx.Add(cell);
        mtx.Sort(MatrixSortX);

        var mtxJoin = new List<int[]>();
        for (int i = 0, length = mtx.Count; i < length; i++)
        {
            if (mtxJoin.Count > 0 && mtxJoin[^1][1] == mtx[i][0] && mtxJoin[^1][2] == mtx[i][2])
                mtxJoin[^1][1] = mtx[i][1];
            else
                mtxJoin.Add(mtx[i]);
        }

        return mtxJoin;
    }

    protected static int MatrixSortDepth(int[] a, int[] b)
    {
        return (a[2] == b[2] && a[0] > b[0]) || a[2] > b[2] ? 1 : -1;
    }

    protected static int MatrixSortX(int[] a, int[] b)
    {
        return a[0] > b[0] ? 1 : -1;
    }

    protected static int[] MatrixTrimWidth(int[] a, int[] b)
    {
        if (a[0] >= b[0] && a[0] < b[1] || a[1] >= b[0] && a[1] < b[1])
        {
            if (a[0] >= b[0] && a[0] < b[1])
                a[0] = b[1];
            else
                a[1] = b[0];
        }

        return a;
    }

    protected static List<int[]> UpdateAttachArea(List<int[]> mtx, int[] point, int[] size)
    {
        mtx.Sort(MatrixSortDepth);

        int[] cell = { point[0], point[0] + size[0], point[1] + size[1] };
        for (int i = 0, length = mtx.Count; i < length; i++)
            if (mtx.Count - 1 >= i)
            {
                if (cell[0] <= mtx[i][0] && mtx[i][1] <= cell[1])
                    mtx.RemoveAt(i);
                else
                    mtx[i] = MatrixTrimWidth(mtx[i], cell);
            }

        return MatrixJoin(mtx, cell);
    }
}
