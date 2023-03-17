using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media.Animation;

namespace AE.CoreWPF;

public class AnimatedFlexItemsControl : FlexItemsControl
{
    public static readonly DependencyProperty AnimationDurationProperty =
        DependencyProperty.Register(nameof(AnimationDuration), typeof(Duration), typeof(AnimatedFlexItemsControl), 
            new PropertyMetadata(new Duration(TimeSpan.FromSeconds(0.2)))
        );

    public Duration AnimationDuration
    {
        get => (Duration)GetValue(AnimationDurationProperty);
        set => SetValue(AnimationDurationProperty, value);
    }

    private bool ignoreAnimation;
    private bool requestUpdate;
    protected AnimationManager AnimationManager { get; }

    public AnimatedFlexItemsControl()
    {
        Opacity = 0;
        AnimationManager = new AnimationManager(MarginProperty);
        AnimationManager.OnCompleted += OnAnimationManagerCompleted;
    }

    private void OnAnimationManagerCompleted(object sender, EventArgs eventArgs)
    {
        if (requestUpdate)
        {
            requestUpdate = false;
            Update();
        }

        Opacity = 1;
    }

    public override void Update()
    {
        base.Update();
        AnimationManager.Start();
    }

    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        ignoreAnimation = true;
        base.OnItemsSourceChanged(oldValue, newValue);
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        ignoreAnimation = true;
        base.OnItemsChanged(e);
    }

    protected override void HandleChildDesiredSizeChanged(UIElement child)
    {
        if (AnimationManager.IsRunning)
            requestUpdate = true;
        else
            base.HandleChildDesiredSizeChanged(child);
    }

    protected override void HandleRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        if (AnimationManager.IsRunning)
            requestUpdate = true;
        else
            base.HandleRenderSizeChanged(sizeInfo);
    }

    protected override void HandleUpdate(FrameworkElement element)
    {
        if (element != null)
        {
            if (ignoreAnimation)
                element.Opacity = 0;

            void complite()
            {
                Update();
                element.Opacity = 1;
            }

            if (element.IsLoaded) 
                complite();
            else
                element.Loaded += (s, e) =>
                {
                    complite();
                    ignoreAnimation = false;
                };
        }
    }

    protected override void SetPosition(FrameworkElement element, int newTop, int newLeft)
    {
        if (ignoreAnimation)
        {
            base.SetPosition(element, newTop, newLeft);
            return;
        }

        if (element != null)
            AnimationManager.Enqueue(element, CreateAnimation(element, newTop, newLeft));
    }

    protected virtual AnimationTimeline CreateAnimation(FrameworkElement element, int newTop, int newLeft)
    {
        return new ThicknessAnimation
        {
            From = element.Margin,
            To = new Thickness(newLeft, newTop, 0, 0),
            Duration = AnimationDuration
        };
    }
}