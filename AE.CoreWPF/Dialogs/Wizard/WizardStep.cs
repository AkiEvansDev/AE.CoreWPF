using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using ModernWpf.Controls;

namespace AE.CoreWPF.Dialogs.Wizard;

internal class WizardStep : IWizardStep
{
	public string Title { get; }
	public string Prev { get; }
	public string Next { get; }

	public SimpleStackPanel Panel { get; }
	public IDictionary<string, Func<object>> Values { get; }

	public WizardStep(string title, string prev, string next)
	{
		Title = title;
		Prev = prev;
		Next = next;

		Panel = new SimpleStackPanel
		{
			Spacing = DisplayHelper.Settings.Border,
			Orientation = Orientation.Vertical,
			Margin = new Thickness(DisplayHelper.Settings.MaxSpace, DisplayHelper.Settings.Space, DisplayHelper.Settings.MaxSpace, DisplayHelper.Settings.Space),
		};
		Values = new Dictionary<string, Func<object>>();
	}

	public UIElement Layout()
	{
		return Panel;
	}

	public void Init(IDictionary<string, object> parameters)
	{
		if (parameters == null)
			return;

		foreach (var param in parameters)
		{
			Values.Remove(param.Key);
			Values.Add(param.Key, () => param.Value);
		}
	}

	public IDictionary<string, object> Result()
	{
		return Values.ToDictionary(v => v.Key, v => v.Value());
	}

	public void BeforeOpen() { }

	public bool BeforeClose()
	{
		return true;
	}
}

public class WizardStepBuilder
{
	public static WizardStepBuilder Create(string title, string prev = "&=dialog_prev", string next = "&=dialog_next")
	{
		return new WizardStepBuilder(title, prev, next);
	}

	private WizardStep Step { get; }

	private WizardStepBuilder(string title, string prev, string next)
	{
		Step = new WizardStep(title, prev, next);
	}

	public WizardStepBuilder CreateTextBlock(string text, double? fontSize = null, FontWeight? fontWeight = null)
	{
		Step.Panel.Children.Add(DisplayHelper.CreateTextBlock(text, fontSize, fontWeight));
		return this;
	}

	public WizardStepBuilder CreateSecondaryTextBlock(string text, double? fontSize = null, FontWeight? fontWeight = null)
	{
		Step.Panel.Children.Add(DisplayHelper.CreateSecondaryTextBlock(text, fontSize, fontWeight));
		return this;
	}

	public WizardStepBuilder CreateTextBox(string name, string value = null, double? size = null, double? fontSize = null, string pattern = null, bool focus = false)
	{
		var element = DisplayHelper.CreateTextBoxVariant(size, null, fontSize, pattern);
		element.Background = DisplayHelper.Settings.TertiaryBackgroundBrush;

		element.Text = value;

		if (focus)
			element.Loaded += (s, e) => element.Focus();

		Step.Panel.Children.Add(element);
		Step.Values.Add(name, () => element.Text);

		return this;
	}

	public WizardStepBuilder CreateCheckBox(string name, string text, bool value = false, double? size = null, double? fontSize = null, FontWeight? fontWeight = null)
	{
		var panel = new SimpleStackPanel
		{
			Spacing = DisplayHelper.Settings.MinSpace,
			Orientation = Orientation.Horizontal,
		};

		var element = DisplayHelper.CreateCheckBox(size);

		element.IsChecked = value;

		panel.Children.Add(element);
		panel.Children.Add(DisplayHelper.CreateSecondaryTextBlock(text, fontSize, fontWeight));

		Step.Panel.Children.Add(panel);
		Step.Values.Add(name, () => element.IsChecked);

		return this;
	}

	public IWizardStep Build()
	{
		return Step;
	}
}



