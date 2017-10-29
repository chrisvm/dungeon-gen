using System;
using System.Collections.Generic;
using dungeon_gen_lib.Bsp;

namespace dungeon_gen_lib.Room
{
	public class RoomCreator
	{
		protected readonly Random Random;
		
		public RoomCreator(Random random)
		{
			Random = random;
		}
		
		public void CreateRooms(BspNode bspTree)
		{
			var leafNodes = _GetLeafNodes(bspTree);
			foreach (var leaf in leafNodes) {
				leaf.room = _CreateRoom(leaf.bbox, Random);
			}
		}
		
		protected static BoundaryBox _CreateRoom(BoundaryBox bbox, Random random)
		{
			var newSize = bbox.size * (random.Next(50, 90) / 100.0);
			var newPos = (bbox.size - newSize) * (random.Next(30, 80) / 100.0);
			return new BoundaryBox(newPos + bbox.position, newSize);
		}
		
		/// <summary>
		/// Gets all the leaf nodes from a given BspNode tree.
		/// </summary>
		/// <param name="bspTree"></param>
		/// <returns>An instance of IEnumerable with all the leaf nodes.</returns>
		protected static IEnumerable<BspNode> _GetLeafNodes(BspNode bspTree)
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