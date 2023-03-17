using System;
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

    private bool requestUpdate;
    protected AnimationManager AnimationManager { get; }

    public AnimatedFlexItemsControl()
    {
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
    }

    public override void Update()
    {
        base.Update();
        AnimationManager.Start();
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
            if (element.IsLoaded)
                Update();
            else
                element.Loaded += (s, e) =>
                {
                    Update();
                };
        }
    }

    protected override void SetPosition(FrameworkElement element, int newTop, int newLeft)
    {
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