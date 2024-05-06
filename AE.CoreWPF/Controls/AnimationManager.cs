using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;

namespace AE.CoreWPF.Controls;

public class AnimationItem
{
	public bool Cancel { get; set; }

	public FrameworkElement Element { get; set; }
	public AnimationTimeline Animation { get; set; }

	public AnimationItem(FrameworkElement element, AnimationTimeline animation)
	{
		Element = element;
		Animation = animation;
	}
}

public class AnimationManager
{
	public event EventHandler Completed;

	private readonly DependencyProperty AnimationProperty;
	private readonly List<AnimationItem> ActiveAnimations;
	private readonly Queue<AnimationItem> AnimationQueue;

	public bool IsRunning => ActiveAnimations.Any();

	public AnimationManager(DependencyProperty animationProperty)
	{
		AnimationProperty = animationProperty;
		ActiveAnimations = new List<AnimationItem>();
		AnimationQueue = new Queue<AnimationItem>();
	}

	public void Enqueue(FrameworkElement element, AnimationTimeline animation)
	{
		if (element != null && animation != null)
		{
			AnimationQueue.Enqueue(new AnimationItem(element, animation));
		}
	}

	public void Start()
	{
		while (AnimationQueue.Count != 0)
		{
			var animationItem = AnimationQueue.Dequeue();
			if (animationItem != null && !animationItem.Cancel && animationItem.Element.IsLoaded)
			{
				ClearActiveAnimations(animationItem);
				HandleAnimationEvent(animationItem);

				animationItem.Element.BeginAnimation(AnimationProperty, animationItem.Animation);
			}
		}
	}

	protected void ClearActiveAnimations(AnimationItem animationItem)
	{
		foreach (var activeAnimation in ActiveAnimations.ToArray())
		{
			if (activeAnimation.Element.Equals(animationItem.Element))
				ActiveAnimations.Remove(activeAnimation);
		}
	}

	protected void HandleAnimationEvent(AnimationItem animationItem)
	{
		foreach (var a in ActiveAnimations.ToArray())
		{
			if (a.Element.Equals(animationItem.Element))
				ActiveAnimations.Remove(a);
		}

		ActiveAnimations.Add(animationItem);
		animationItem.Animation.Completed += (s, e) =>
		{
			ActiveAnimations.Remove(animationItem);
			if (!ActiveAnimations.Any())
				Completed?.Invoke(null, EventArgs.Empty);
		};
	}
}
