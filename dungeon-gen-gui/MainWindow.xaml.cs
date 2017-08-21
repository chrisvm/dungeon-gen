using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using dungeon_gen_lib.Bsp;
using dungeon_gen_lib.Rendering;

namespace dungeon_gen_gui
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		
		private BinarySpacePartition _bsp;
		private BinarySpacePartition Bsp => _bsp ?? (_bsp = new BinarySpacePartition {
			MinimumSideSize = 100,
			PrintDebug = false
		});
		
		public MainWindow()
		{
			InitializeComponent();
		}

		private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
		{
			var mapWidth = int.Parse(WidthTextBox.Text);
			var mapHeight = int.Parse(HeighTextBox.Text);
			if (mapWidth == 0 || mapHeight == 0) return;
			
			var tree = Bsp.Partition(new BoundaryBox(new Vector2(0, 0), new Vector2(mapWidth, mapHeight)));
			var bitmap = BitmapRenderer.Instance.Render(tree);
			MapImage.Width = bitmap.Width;
			MapImage.Height = bitmap.Height;
			MapImage.Source = BitmapToImageSource(bitmap);
		}
		
		private BitmapImage BitmapToImageSource(Image bitmap)
		{
			using (var memory = new MemoryStream())
			{
				bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
				memory.Position = 0;
				var bitmapimage = new BitmapImage();
				bitmapimage.BeginInit();
				bitmapimage.StreamSource = memory;
				bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapimage.EndInit();
				return bitmapimage;
			}
		}
	}
}
