using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AE.CoreWPF.Controls;

public delegate void AsyncImageLoadEventHandler(AsyncImage image);

public class AsyncImage : Image
{
	public static readonly DependencyProperty ImagePathProperty =
		DependencyProperty.Register(nameof(ImagePath), typeof(string), typeof(AsyncImage), new PropertyMetadata(async (o, e) => await ((AsyncImage)o).LoadImageAsync((string)e.NewValue)));

	public event AsyncImageLoadEventHandler OnImageLoad;

	public string ImagePath
	{
		get => (string)GetValue(ImagePathProperty);
		set => SetValue(ImagePathProperty, value);
	}

	public int DecodePixelWidth { get; set; }
	public int DecodePixelHeight { get; set; }

	private async Task LoadImageAsync(string imagePath)
	{
		Source = await Task.Run(() =>
		{
			using var stream = File.OpenRead(imagePath);
			var bi = new BitmapImage();

			bi.BeginInit();

			bi.CacheOption = BitmapCacheOption.OnLoad;
			bi.DecodePixelWidth = DecodePixelWidth;
			bi.DecodePixelHeight = DecodePixelHeight;
			bi.StreamSource = stream;

			bi.EndInit();
			bi.Freeze();

			return bi;
		});
		OnImageLoad?.Invoke(this);
	}
}
