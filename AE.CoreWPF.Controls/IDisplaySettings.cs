using System.Windows.Media;

namespace AE.CoreWPF.Controls;

public interface IDisplaySettings
{
	double MaxBorder { get; }
	double Border { get; }
	double MinBorder { get; }

	double MaxSpace { get; }
	double Space { get; }
	double MinSpace { get; }

	double Round { get; }

	public Color Transperent { get; }
	public Brush TransperentBrush { get; }

	public Color Background { get; }
	public Color SecondaryBackground { get; }
	public Color TertiaryBackground { get; }

	public Brush BackgroundBrush { get; }
	public Brush SecondaryBackgroundBrush { get; }
	public Brush TertiaryBackgroundBrush { get; }

	public Color Foreground { get; }
	public Color SecondaryForeground { get; }
	public Color TertiaryForeground { get; }

	public Brush ForegroundBrush { get; }
	public Brush SecondaryForegroundBrush { get; }
	public Brush TertiaryForegroundBrush { get; }

	public Color Accent { get; }
	public Brush AccentBrush { get; }

	public Color SelectBackground { get; }
	public Color SelectStroke { get; }

	public Brush SelectBackgroundBrush { get; }
	public Brush SelectStrokeBrush { get; }

	public Color MessageColor { get; }
	public Color WarningColor { get; }
	public Color ErrorColor { get; }
	public Color SuccessColor { get; }

	public Brush MessageColorBrush { get; }
	public Brush WarningColorBrush { get; }
	public Brush ErrorColorBrush { get; }
	public Brush SuccessColorBrush { get; }
}
