using System;
using System.Collections.Generic;
using System.Linq;
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
			
			// create rooms
			var random = new Random();
			var roomCreator = new RoomCreator(random);
			roomCreator.CreateRooms(nodeTree);
			
			// collect only the nodes which children are all leafs
			var leafParents = nodeTree.AllNodesBeneath().Where((node, i) => {
				var childrenAreLeafs = node.Children.Count != 0;
				foreach (var child in node.Children) {
					if (child.Children.Count != 0) childrenAreLeafs = false;
				}
				return childrenAreLeafs;
			});
			
			var bspNodes = leafParents as BspNode[] ?? leafParents.ToArray();
			Console.WriteLine($"leafParents.Count = {bspNodes.Length}");
			
			// connect the filtered nodes
			var roomConnector = new RoomConnector(random);
			var zCorridorCount = 0;
			var connections = new List<RoomConnection>();
			foreach (var parent in bspNodes) {
				try {
					var roomConnection = roomConnector.ConnectRooms(parent);
					connections.Add(roomConnection);
				} catch (NotImplementedException) {
					++zCorridorCount;
				}
			}
			
			Console.WriteLine($"Z Corridor Count = {zCorridorCount}");
			Console.WriteLine($"Connection Count = {connections.Count}");

			// render partition
			PrintToBitmap(nodeTree, connections);
		}

		/// <summary>
		/// Renders the given node tree to a png in the Desktop of the 
		/// running user with the name "Example.png".
		/// </summary>
		/// <param name="nodeTree"></param>
		/// <param name="connections"></param>
		private static void PrintToBitmap(BspNode nodeTree, List<RoomConnection> connections)
		{
			var bitmap = new BitmapRenderer().Render(nodeTree);
			var path = System.IO.Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				"Example.png");
			bitmap.Save(path);
		}
	}
}