using System.IO;
using System.Reflection;
using System.Windows.Media;

using AE.Dal;

namespace AE.CoreWPF;

[AESerializable]
public class DisplaySettings : IDisplaySettings
{
	public const string QuicksandFont = "Quicksand";
	public const string Exo2Font = "Exo 2";
	public const string ChakraPetchFont = "Chakra Petch";

	private double border;
	public double Border
	{
		get => border;
		set
		{
			border = value;
			MinBorder = border / 2;
			MaxBorder = border * 2;
		}
	}
	public double MinBorder { get; private set; }
	public double MaxBorder { get; private set; }

	private double space;
	public double Space
	{
		get => space;
		set
		{
			space = value;
			MinSpace = space / 2;
			MaxSpace = space * 2;
		}
	}
	public double MinSpace { get; private set; }
	public double MaxSpace { get; private set; }

	private double round;
	public double Round
	{
		get => round;
		set
		{
			round = value;
			MinRound = round / 2;
			MaxRound = round * 2;
		}
	}
	public double MinRound { get; private set; }
	public double MaxRound { get; private set; }

	private Color transperent;
	public Color Transperent
	{
		get => transperent;
		set
		{
			transperent = value;
			TransperentBrush = value.ToBrush();
		}
	}
	public Brush TransperentBrush { get; private set; }

	private Color background;
	public Color Background
	{
		get => background;
		set
		{
			background = value;
			BackgroundBrush = value.ToBrush();
		}
	}
	public Brush BackgroundBrush { get; private set; }

	private Color secondaryBackground;
	public Color SecondaryBackground
	{
		get => secondaryBackground;
		set
		{
			secondaryBackground = value;
			SecondaryBackgroundBrush = value.ToBrush();
		}
	}
	public Brush SecondaryBackgroundBrush { get; private set; }

	private Color tertiaryBackground;
	public Color TertiaryBackground
	{
		get => tertiaryBackground;
		set
		{
			tertiaryBackground = value;
			TertiaryBackgroundBrush = value.ToBrush();
		}
	}
	public Brush TertiaryBackgroundBrush { get; private set; }

	private Color selectBackground;
	public Color SelectBackground
	{
		get => selectBackground;
		set
		{
			selectBackground = value;
			SelectBackgroundBrush = value.ToBrush();
		}
	}
	public Brush SelectBackgroundBrush { get; private set; }

	private Color foreground;
	public Color Foreground
	{
		get => foreground;
		set
		{
			foreground = value;
			ForegroundBrush = value.ToBrush();
		}
	}
	public Brush ForegroundBrush { get; private set; }

	private Color secondaryForeground;
	public Color SecondaryForeground
	{
		get => secondaryForeground;
		set
		{
			secondaryForeground = value;
			SecondaryForegroundBrush = value.ToBrush();
		}
	}
	public Brush SecondaryForegroundBrush { get; private set; }

	private Color tertiaryForeground;
	public Color TertiaryForeground
	{
		get => tertiaryForeground;
		set
		{
			tertiaryForeground = value;
			TertiaryForegroundBrush = value.ToBrush();
		}
	}
	public Brush TertiaryForegroundBrush { get; private set; }

	private Color accent;
	public Color Accent
	{
		get => accent;
		set
		{
			accent = value;
			AccentBrush = value.ToBrush();
		}
	}
	public Brush AccentBrush { get; private set; }

	private Color stroke;
	public Color Stroke
	{
		get => stroke;
		set
		{
			stroke = value;
			StrokeBrush = value.ToBrush();
		}
	}
	public Brush StrokeBrush { get; private set; }

	private Color messageColor;
	public Color MessageColor
	{
		get => messageColor;
		set
		{
			messageColor = value;
			MessageColorBrush = value.ToBrush();
		}
	}
	public Brush MessageColorBrush { get; private set; }

	private Color warningColor;
	public Color WarningColor
	{
		get => warningColor;
		set
		{
			warningColor = value;
			WarningColorBrush = value.ToBrush();
		}
	}
	public Brush WarningColorBrush { get; private set; }

	private Color errorColor;
	public Color ErrorColor
	{
		get => errorColor;
		set
		{
			errorColor = value;
			ErrorColorBrush = value.ToBrush();
		}
	}
	public Brush ErrorColorBrush { get; private set; }

	private Color successColor;
	public Color SuccessColor
	{
		get => successColor;
		set
		{
			successColor = value;
			SuccessColorBrush = value.ToBrush();
		}
	}
	public Brush SuccessColorBrush { get; private set; }

	private Color closeColor;
	public Color CloseColor
	{
		get => closeColor;
		set
		{
			closeColor = value;
			CloseColorBrush = value.ToBrush();
		}
	}
	public Brush CloseColorBrush { get; private set; }

	public FontFamily FontFamily { get; set; }
	public FontFamily FontFamilyBold { get; set; }
	public FontFamily FontFamilyMedium { get; set; }
	public FontFamily FontFamilyLight { get; set; }

	public double ControlSize { get; set; } = 30;//= 28;
	public double ControlScale { get; set; } = 1;

	public double ControlSizeCompact { get; set; } = 24;//= 20;
	public double ControlScaleCompact { get; set; } = 0.8;

	public double ControlSizeCompact1 { get; set; } = 26;//= 22;
	public double ControlScaleCompact1 { get; set; } = 0.84;

	public double ControlSizeCompact2 { get; set; } = 28;//= 24;
	public double ControlScaleCompact2 { get; set; } = 0.88;

	public double ControlOpacity { get; set; } = 1;
	public double ControlSecondaryOpacity { get; set; } = 0.8;
	public double ControlDisabledOpacity { get; set; } = 0.2;

	public double FontSize { get; set; } = 15;
	public double FontSizeCompact { get; set; } = 13;
	public double FontSizeCompact2 { get; set; } = 14;

	public byte ColorOpacity1 { get; set; } = 200;
	public byte ColorOpacity2 { get; set; } = 100;
	public byte ColorOpacity3 { get; set; } = 50;
	public byte ColorOpacity4 { get; set; } = 20;
	public byte ColorOpacity5 { get; set; } = 10;

	public string NamePattern { get; set; } = "^[a-zA-Z][a-zA-Z0-9 #]*$";
	public string TextPattern { get; set; } = "^[a-zA-Zа-яА-Я0-9 #.,!?&@%$()\\[\\]_\\-+/*=<>]*$";
	public string PathPattern { get; set; } = "^[a-zA-Z]:[a-zA-Zа-яА-Я0-9 #.\\\\_\\-()\\[\\]]*$";
	public string BytePattern { get; set; } = "^[0-9]*$";
	public string IntegerPattern { get; set; } = "^[0-9-][0-9]*$";
	public string DoublePattern { get; set; } = "^[0-9-][0-9.]*$";

	public DisplaySettings(double border, double space, double round, ColorPath background, ColorPath secondaryBackground, ColorPath tertiaryBackground, ColorPath foreground, ColorPath secondaryForeground, ColorPath tertiaryForeground, ColorPath accent, ColorPath stroke, ColorPath messageColor, ColorPath warningColor, ColorPath errorColor, ColorPath successColor, string font = ChakraPetchFont, byte colorOpacity1 = 200, byte colorOpacity2 = 100, byte colorOpacity3 = 50, byte colorOpacity4 = 20, byte colorOpacity5 = 10)
	{
		ColorOpacity1 = colorOpacity1;
		ColorOpacity2 = colorOpacity2;
		ColorOpacity3 = colorOpacity3;
		ColorOpacity4 = colorOpacity4;
		ColorOpacity5 = colorOpacity5;

		Border = border;
		Space = space;
		Round = round;

		Transperent = Color.FromArgb(0, 0, 0, 0);

		Background = background.ToColor().ToColor();
		SecondaryBackground = secondaryBackground.ToColor().ToColor();
		TertiaryBackground = tertiaryBackground.ToColor().ToColor();
		SelectBackground = TertiaryBackground.WithAlpha(ColorOpacity1);

		Foreground = foreground.ToColor().ToColor();
		SecondaryForeground = secondaryForeground.ToColor().ToColor();
		TertiaryForeground = tertiaryForeground.ToColor().ToColor();

		Accent = accent.ToColor().ToColor();

		Stroke = stroke.ToColor().ToColor();

		MessageColor = messageColor.ToColor().ToColor();
		WarningColor = warningColor.ToColor().ToColor();
		ErrorColor = errorColor.ToColor().ToColor();
		SuccessColor = successColor.ToColor().ToColor();

		CloseColor = new ColorPath(ColorKey.CrimsonRed, ColorType.Shade, 1).ToColor().ToColor();

		var path = Path.Combine(
			Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase),
			"Fonts"
		);

		FontFamily = new FontFamily($"{path}/#{font} Medium");
		FontFamilyBold = new FontFamily($"{path}/#{font} Bold");
		FontFamilyMedium = new FontFamily($"{path}/#{font} SemiBold");
		FontFamilyLight = new FontFamily($"{path}/#{font} Light");
	}

	public static DisplaySettings Default(ColorPath? accent = null)
	{
		return new DisplaySettings(
			border: 2,
			space: 6,
			round: 4,
			new ColorPath(ColorKey.SilverNight, ColorType.Shade, 10),
			new ColorPath(ColorKey.SilverNight, ColorType.Shade, 9),
			new ColorPath(ColorKey.SilverNight, ColorType.Shade, 8),
			new ColorPath(ColorKey.SilverNight, ColorType.Tint, 10, 200),
			new ColorPath(ColorKey.SilverNight, ColorType.Tint, 2, 200),
			new ColorPath(ColorKey.SilverNight, ColorType.Shade, 2, 200),
			accent ?? new ColorPath(ColorKey.PaleViolet, ColorType.Shade, 4),
			new ColorPath(ColorKey.SilverNight, ColorType.Shade, 7, 200),
			new ColorPath(ColorKey.BlueEyes, ColorType.Tint, 2),
			new ColorPath(ColorKey.RipeMango, ColorType.Tint, 2),
			new ColorPath(ColorKey.CrimsonRed, ColorType.Tint, 2),
			new ColorPath(ColorKey.HarlequinGreen, ColorType.Tint, 2)
		);
	}

	public static DisplaySettings Create(ColorKey mainKey, ColorKey accentKey)
	{
		return new DisplaySettings(
			border: 2,
			space: 6,
			round: 4,
			new ColorPath(mainKey, ColorType.Shade, mainKey == ColorKey.SilverNight ? 10 : 9),
			new ColorPath(mainKey, ColorType.Shade, mainKey == ColorKey.SilverNight ? 9 : 8),
			new ColorPath(mainKey, ColorType.Shade, mainKey == ColorKey.SilverNight ? 8 : 7),
			new ColorPath(mainKey, ColorType.Tint, mainKey == ColorKey.SilverNight ? 10 : 9, 200),
			new ColorPath(mainKey, ColorType.Tint, mainKey == ColorKey.SilverNight ? 2 : 6, 200),
			new ColorPath(mainKey, ColorType.Shade, mainKey == ColorKey.SilverNight ? 2 : 6, 200),
			new ColorPath(accentKey, ColorType.Shade, 4),
			new ColorPath(mainKey, ColorType.Shade, mainKey == ColorKey.SilverNight ? 7 : 6, 200),
			new ColorPath(ColorKey.BlueEyes, ColorType.Tint, 2),
			new ColorPath(ColorKey.RipeMango, ColorType.Tint, 2),
			new ColorPath(ColorKey.CrimsonRed, ColorType.Tint, 2),
			new ColorPath(ColorKey.HarlequinGreen, ColorType.Tint, 2)
		);
	}
}