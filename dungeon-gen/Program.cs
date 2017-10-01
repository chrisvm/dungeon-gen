using System;
using dungeon_gen_lib.Room;
using dungeon_gen_lib.Bsp;
using dungeon_gen_lib.Rendering;

namespace dungeon_gen
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var mapGenerator = new BinarySpacePartition {
				MinimumSideSize = 100,
				PrintDebug = false
			};
			
			// bsp partition
			var bbox = new BoundaryBox(new Vector2(0, 0), new Vector2(800, 800));
			var nodeTree = mapGenerator.Partition(bbox);
			Console.WriteLine($"Tree.Children.Count = {nodeTree.Children.Count}");
			
			// create rooms
			var random = new Random();
			var roomCreator = new RoomCreator(random);
			roomCreator.CreateRooms(nodeTree);
			
			// render partition
			PrintToBitmap(nodeTree);
		}
		
		private static void PrintToBitmap(BspNode nodeTree)
		{
			var bitmap = new BitmapRenderer().Render(nodeTree);
			var path = System.IO.Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				"Example.png");
			bitmap.Save(path);
		}
	}
}