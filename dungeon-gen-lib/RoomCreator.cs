using System;
using System.Collections.Generic;
using dungeon_gen_lib.Bsp;

namespace dungeon_gen_lib
{
	public class RoomCreator
	{
		private static readonly Random Random = new Random();
		
		public static void CreateRooms(BspNode bspTree)
		{
			var leafNodes = GetLeafNodes(bspTree);
			foreach (var leaf in leafNodes) {
				leaf.Room = CreateRoom(leaf.BBox);
			}
		}

		protected static BoundaryBox CreateRoom(BoundaryBox bbox)
		{
			var newSize = bbox.Size * (Random.Next(50, 90) / 100.0);
			var newPos = (bbox.Size - newSize) * (Random.Next(30, 80) / 100.0);
			return new BoundaryBox(newPos + bbox.Position, newSize);
		}

		private static IEnumerable<BspNode> GetLeafNodes(BspNode bspTree)
		{
			var list = new List<BspNode>();
			var queue = new Queue<BspNode>();
			queue.Enqueue(bspTree);
			
			while (queue.Count != 0) {
				var node = queue.Dequeue();
				if (node.Children.Count == 0) {
					list.Add(node);
					continue;
				}
				foreach (var child in node.Children) {
					queue.Enqueue(child);
				}
			}
			
			return list;
		}
	}
}