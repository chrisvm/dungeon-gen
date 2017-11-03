using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
		private static readonly RoomConnector _roomConnector = new RoomConnector(_random);
		
		private static readonly BinarySpacePartition _bsp = new BinarySpacePartition {
			MinimumSideSize = 100,
			PrintDebug = false 
		};
		
		public MainWindow()
		{
			InitializeComponent();
			InitializeDefaultValues(this);
		}

		private static void InitializeDefaultValues(MainWindow mainWindow)
		{
			mainWindow.WidthTextBox.Text = "400";
			mainWindow.HeighTextBox.Text = "400";
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
			
			var leafParents = tree.AllNodesBeneath().Where((node, i) => {
				var childrenAreLeafs = node.Children.Count != 0;
				foreach (var child in node.Children) {
					if (child.Children.Count != 0) childrenAreLeafs = false;
				}
				return childrenAreLeafs;
			});
			
			var bspNodes = leafParents as BspNode[] ?? leafParents.ToArray();
			Console.WriteLine($"leafParents.Count = {bspNodes.Length}");
			
			// connect the filtered nodes
			var connections = new List<RoomConnection>();
			foreach (var parent in bspNodes) {
				try {
					var roomConnection = _roomConnector.ConnectRooms(parent);
					connections.Add(roomConnection);
				} catch (NotImplementedException) {}
			}
			
			var bitmap = new Bitmap((int) tree.bbox.size.x, (int) tree.bbox.size.y);
			_bitmapRenderer.Render(tree, bitmap);
			_bitmapRenderer.Render(connections, bitmap);
			
			MapImage.Width = bitmap.Width;
			MapImage.Height = bitmap.Height;
			MapImage.Source = BitmapRenderer.BitmapToImageSource(bitmap);
		}
	}
}
