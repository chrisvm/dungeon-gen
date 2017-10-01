using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using dungeon_gen_lib.Room;
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
		private RoomCreator _roomCreator;
		
		public MainWindow()
		{
			InitializeComponent();
			var random = new Random();
			_roomCreator = new RoomCreator(random);
			InitializeDefaultValues();
		}

		private void InitializeDefaultValues()
		{
			WidthTextBox.Text = "400";
			HeighTextBox.Text = "400";
		}


		private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (WidthTextBox.Text == "" && HeighTextBox.Text == "") return;
			
			var mapWidth = int.Parse(WidthTextBox.Text);
			var mapHeight = int.Parse(HeighTextBox.Text);
			if (mapWidth == 0 || mapHeight == 0) return;
			var tree = Bsp.Partition(new BoundaryBox(
				                         new Vector2(0, 0), 
				                         new Vector2(mapWidth, mapHeight)));
			_roomCreator.CreateRooms(tree);
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
