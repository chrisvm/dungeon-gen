using System;
using System.Collections.Generic;
using dungeon_gen.Bsp;

namespace dungeon_gen
{
	public class RoomCreator
	{
		private const double MinRoomDelta = 0.10;
		private const double MaxRoomDelta = 0.25;
		private const int MinRange = (int) (MinRoomDelta * 100);
		private const int MaxRange = (int) (MaxRoomDelta * 100);
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
			var sides = new double[4];
			var isVertical = true;
			for (var index = 0; index < 4; index += 1) {
				var delta = Random.Next(MinRange, MaxRange) / 100.0;
				if (isVertical) {
					sides[index] = delta * bbox.Size.Y;
				} else {
					sides[index] = delta * bbox.Size.X;
				}
				isVertical = !isVertical;
			}
			
			var pos = new Vector2((int) (bbox.Position.X + sides[3]), (int) (bbox.Position.Y + sides[0]));
			var size = new Vector2((int) (bbox.Size.X - sides[1]), (int) (bbox.Size.Y - sides[2]));
			return new BoundaryBox(pos, size);
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