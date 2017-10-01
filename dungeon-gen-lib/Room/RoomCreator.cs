using System;
using System.Collections.Generic;
using dungeon_gen_lib.Bsp;

namespace dungeon_gen_lib.Room
{
	public class RoomCreator
	{
		protected Random _random;

		protected static BoundaryBox _CreateRoom(BoundaryBox bbox, Random random)
		{
			var newSize = bbox.Size * (random.Next(50, 90) / 100.0);
			var newPos = (bbox.Size - newSize) * (random.Next(30, 80) / 100.0);
			return new BoundaryBox(newPos + bbox.Position, newSize);
		}

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
		
		public RoomCreator(Random random)
		{
			_random = random;
		}
		
		private IEnumerable<BspNode> GetLeafNodes(BspNode bspTree)
		{
			return _GetLeafNodes(bspTree);
		}
		
		public void CreateRooms(BspNode bspTree)
		{
			var leafNodes = _GetLeafNodes(bspTree);
			foreach (var leaf in leafNodes) {
				leaf.Room = _CreateRoom(leaf.BBox, _random);
			}
		}
	}
}