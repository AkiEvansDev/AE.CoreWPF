using System.Windows.Media;

using AE.Dal;

namespace AE.CoreWPF.Controls;

[AESerializable]
public class DisplaySettings : IDisplaySettings
{
	public double MaxBorder { get; set; }
	public double Border { get; set; }
	public double MinBorder { get; set; }

	public double MaxSpace { get; set; }
	public double Space { get; set; }
	public double MinSpace { get; set; }

	public double Round { get; set; }

	public Color Transperent { get; set; }
	public Brush TransperentBrush { get; set; }

	public Color Background { get; set; }
	public Color SecondaryBackground { get; set; }
	public Color TertiaryBackground { get; set; }

	public Brush BackgroundBrush { get; set; }
	public Brush SecondaryBackgroundBrush { get; set; }
	public Brush TertiaryBackgroundBrush { get; set; }

	public Color Foreground { get; set; }
	public Color SecondaryForeground { get; set; }
	public Color TertiaryForeground { get; set; }

	public Brush ForegroundBrush { get; set; }
	public Brush SecondaryForegroundBrush { get; set; }
	public Brush TertiaryForegroundBrush { get; set; }

	public Color Accent { get; set; }
	public Brush AccentBrush { get; set; }

	public Color SelectBackground { get; set; }
	public Color SelectStroke { get; set; }

	public Brush SelectBackgroundBrush { get; set; }
	public Brush SelectStrokeBrush { get; set; }

	public Color MessageColor { get; set; }
	public Color WarningColor { get; set; }
	public Color ErrorColor { get; set; }
	public Color SuccessColor { get; set; }

	public Brush MessageColorBrush { get; set; }
	public Brush WarningColorBrush { get; set; }
	public Brush ErrorColorBrush { get; set; }
	public Brush SuccessColorBrush { get; set; }

	public DisplaySettings(double border, double space, double round, ColorPath background, ColorPath secondaryBackground, ColorPath tertiaryBackground, ColorPath foreground, ColorPath secondaryForeground, ColorPath tertiaryForeground, ColorPath accent, ColorPath selectBackground, ColorPath selectStroke, ColorPath messageColor, ColorPath warningColor, ColorPath errorColor, ColorPath successColor)
	{
		MaxBorder = border * 2;
		Border = border;
		MinBorder = border / 2;

		MaxSpace = space * 2;
		Space = space;
		MinSpace = space / 2;

		Round = round;

		Transperent = Color.FromArgb(0, 0, 0, 0);
		TransperentBrush = Transperent.ToBrush();

		Background = background.ToColor().ToColor();
		SecondaryBackground = secondaryBackground.ToColor().ToColor();
		TertiaryBackground = tertiaryBackground.ToColor().ToColor();

		BackgroundBrush = Background.ToBrush();
		SecondaryBackgroundBrush = SecondaryBackground.ToBrush();
		TertiaryBackgroundBrush = TertiaryBackground.ToBrush();

		Foreground = foreground.ToColor().ToColor();
		SecondaryForeground = secondaryForeground.ToColor().ToColor();
		TertiaryForeground = tertiaryForeground.ToColor().ToColor();

		ForegroundBrush = Foreground.ToBrush();
		SecondaryForegroundBrush = SecondaryForeground.ToBrush();
		TertiaryForegroundBrush = TertiaryForeground.ToBrush();

		Accent = accent.ToColor().ToColor();
		AccentBrush = Accent.ToBrush();

		SelectBackground = selectBackground.ToColor().ToColor();
		SelectStroke = selectStroke.ToColor().ToColor();

		SelectBackgroundBrush = SelectBackground.ToBrush();
		SelectStrokeBrush = SelectStroke.ToBrush();

		MessageColor = messageColor.ToColor().ToColor();
		WarningColor = warningColor.ToColor().ToColor();
		ErrorColor = errorColor.ToColor().ToColor();
		SuccessColor = successColor.ToColor().ToColor();

		MessageColorBrush = MessageColor.ToBrush();
		WarningColorBrush = WarningColor.ToBrush();
		ErrorColorBrush = ErrorColor.ToBrush();
		SuccessColorBrush = SuccessColor.ToBrush();
	}
}
