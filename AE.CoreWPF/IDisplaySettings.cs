using System.Windows.Media;

namespace AE.CoreWPF;

public interface IDisplaySettings
{
	double Border { get; }
	double MinBorder { get; }
	double MaxBorder { get; }

	double Space { get; }
	double MinSpace { get; }
	double MaxSpace { get; }

	double Round { get; }
	double MinRound { get; }
	double MaxRound { get; }

	Color Transperent { get; }
	Brush TransperentBrush { get; }

	Color Background { get; }
	Brush BackgroundBrush { get; }

	Color SecondaryBackground { get; }
	Brush SecondaryBackgroundBrush { get; }

	Color TertiaryBackground { get; }
	Brush TertiaryBackgroundBrush { get; }

	Color SelectBackground { get; }
	Brush SelectBackgroundBrush { get; }

	Color Foreground { get; }
	Brush ForegroundBrush { get; }

	Color SecondaryForeground { get; }
	Brush SecondaryForegroundBrush { get; }

	Color TertiaryForeground { get; }
	Brush TertiaryForegroundBrush { get; }

	Color Accent { get; }
	Brush AccentBrush { get; }

	Color Stroke { get; }
	Brush StrokeBrush { get; }

	Color MessageColor { get; }
	Brush MessageColorBrush { get; }

	Color WarningColor { get; }
	Brush WarningColorBrush { get; }

	Color ErrorColor { get; }
	Brush ErrorColorBrush { get; }

	Color SuccessColor { get; }
	Brush SuccessColorBrush { get; }

	FontFamily FontFamily { get; }
	FontFamily FontFamilyBold { get; }
	FontFamily FontFamilyMedium { get; }
	FontFamily FontFamilyLight { get; }

	double ControlSize { get; }
	double ControlScale { get; }

	double ControlSizeCompact { get; }
	double ControlScaleCompact { get; }

	double ControlSizeCompact1 { get; }
	double ControlScaleCompact1 { get; }

	double ControlSizeCompact2 { get; }
	double ControlScaleCompact2 { get; }

	double ControlOpacity { get; }
	double ControlSecondaryOpacity { get; }

	double FontSize { get; }
	double FontSizeCompact { get; }

	byte ColorOpacity1 { get; }
	byte ColorOpacity2 { get; }
	byte ColorOpacity3 { get; }

	string NamePattern { get; }
	string TextPattern { get; }
	string PathPattern { get; }
	string BytePattern { get; }
	string IntegerPattern { get; }
	string DoublePattern { get; }
}
