using System;
using System.Windows;
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
		private static readonly BitmapRenderer _bitmapRenderer = new BitmapRenderer();
		private static readonly Random _random = new Random();
		private static readonly RoomCreator _roomCreator = new RoomCreator(_random);
		private static readonly BinarySpacePartition _bsp = new BinarySpacePartition {
			MinimumSideSize = 100,
			PrintDebug = false 
		};
		
		public MainWindow()
		{
			InitializeComponent();
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
			var tree = _bsp.Partition(new BoundaryBox(
				                         new Vector2(0, 0), 
				                         new Vector2(mapWidth, mapHeight)));
			_roomCreator.CreateRooms(tree);
			var bitmap = _bitmapRenderer.Render(tree);
			MapImage.Width = bitmap.Width;
			MapImage.Height = bitmap.Height;
			MapImage.Source = BitmapRenderer.BitmapToImageSource(bitmap);
		}
	}
}
