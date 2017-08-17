using System;
using dungeon_gen.Bsp;

namespace dungeon_gen
{
	public class BspMapGenerator
	{
		public enum SplitDirection { Horizontal, Vertical }

		public float MinimumBoxSize;
		public bool PrintDebug;
		
		private readonly Random _randomInstance = new Random();
		
		public BspNode BinarySpacePartition(BoundaryBox bbox)
		{
			Console.WriteLine($"Recursively splitting {bbox}");
			var map = new BspNode(bbox);
			RecursiveBsp(map);
			return map;
		}

		private void RecursiveBsp(BspNode node)
		{
			if (PrintDebug) {
				Console.WriteLine($"Node.BBox = {node.BBox}");	
			}
			bool xIsMinSize = false, yIsMinSize = false;
			var bbox = node.BBox;
			
			// check horizontal space
			if (bbox.Size.X <= MinimumBoxSize && bbox.Size.X * .25 <= MinimumBoxSize) {
				xIsMinSize = true;
			}
			
			// check vertical space
			if (bbox.Size.Y <= MinimumBoxSize && bbox.Size.Y * .25 <= MinimumBoxSize) {
				yIsMinSize = true;
			}
			
			// if cant devide, finish recursion
			if (xIsMinSize && yIsMinSize) {
				return;
			}
			
			// if can divide in both dir, choose stochastically
			SplitDirection splitDirection;
			if (!xIsMinSize && !yIsMinSize) {
				splitDirection = RandomSplitDirection();	
			} else if (xIsMinSize) {
				splitDirection = SplitDirection.Horizontal;
			} else {
				splitDirection = SplitDirection.Vertical;
			}
			
			// split box base on direction
			BspNode node1, node2;
			if (splitDirection == SplitDirection.Vertical) {
				// random split between Min boxsize and max box size
				var split = _randomInstance.Next(25, 75) / 100.0;
				
				var size1 = new Vector2Int((int)(bbox.Size.X * split), bbox.Size.Y);
				var pos1 = bbox.Position;
				var size2 = new Vector2Int(bbox.Size.X - size1.X, bbox.Size.Y);			
				var pos2 = new Vector2Int(bbox.Position.X + size1.X, bbox.Position.Y);
				
				node1 = new BspNode(node, new BoundaryBox(pos1, size1));
				node.AddChild(node1);
				node2 = new BspNode(node, new BoundaryBox(pos2, size2));
				node.AddChild(node2);
			} else {
				// random split between Min boxsize and max box size
				var split = _randomInstance.Next(25, 75) / 100.0;
				
				var size1 = new Vector2Int(bbox.Size.X, (int)(bbox.Size.Y * split));
				var pos1 = bbox.Position;
				var size2 = new Vector2Int(bbox.Size.X, bbox.Size.Y - size1.Y);
				var pos2 = new Vector2Int(bbox.Position.X, bbox.Position.Y + size1.Y);
				
				node1 = new BspNode(node, new BoundaryBox(pos1, size1));
				node.AddChild(node1);
				node2 = new BspNode(node, new BoundaryBox(pos2, size2));
				node.AddChild(node2);
			}
			
			// recursive exec
			RecursiveBsp(node1);
			RecursiveBsp(node2);
		}

		private SplitDirection RandomSplitDirection()
		{
			var randomDouble = _randomInstance.NextDouble();
			if (randomDouble < 0.5) {
				return SplitDirection.Horizontal;
			}
			return SplitDirection.Vertical;
		}
	}
}