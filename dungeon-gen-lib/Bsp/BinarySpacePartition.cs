using System;

namespace dungeon_gen_lib.Bsp
{
	/// <summary>
	/// Describes a spliting axis.
	/// </summary>
	public enum SplitDirection { Horizontal, Vertical }
	
	public class BinarySpacePartition
	{
		private const double MinPartitionRange = 0.25;
		private const double MaxPartitionRange = 0.75;
		
		public float MinimumSideSize { get; set; }
		public bool PrintDebug { get; set; }
		
		private readonly Random _randomInstance = new Random();
		
		/// <summary>
		/// Partitions a given boundary box recursively using bsp.
		/// </summary>
		/// <param name="bbox"></param>
		/// <returns></returns>
		public BspNode Partition(BoundaryBox bbox)
		{
			Console.WriteLine($"Recursively splitting {bbox}");
			var map = new BspNode(bbox);
			RecursiveBsp(map, _randomInstance, MinimumSideSize);
			return map;
		}
		
		private static void RecursiveBsp(BspNode node, 
		                                 Random randomInstance, 
		                                 float minimumSideSize)
		{
			while (true) {
				bool xIsMinSize = false, yIsMinSize = false;
				var bbox = node.BBox;

				// check horizontal space supports partitioning
				var supportsMinDivision = bbox.Size.X * MinPartitionRange > minimumSideSize;
				var supportsMaxDivision = bbox.Size.X - bbox.Size.X * MaxPartitionRange > minimumSideSize;
				if (bbox.Size.X <= minimumSideSize && !supportsMaxDivision && !supportsMinDivision) {
					xIsMinSize = true;
				}

				// check vertical space supports partitioning
				supportsMinDivision = bbox.Size.Y * MinPartitionRange > minimumSideSize;
				supportsMaxDivision = bbox.Size.Y - bbox.Size.X * MaxPartitionRange > minimumSideSize;
				if (bbox.Size.Y <= minimumSideSize && !supportsMaxDivision && !supportsMinDivision) {
					yIsMinSize = true;
				}

				// if cant divide, finish recursion
				if (xIsMinSize && yIsMinSize) {
					return;
				}

				// if can divide in both dir, choose stochastically
				SplitDirection splitDirection;
				if (!xIsMinSize && !yIsMinSize) {
					splitDirection = RandomSplitDirection(randomInstance);
				}
				else if (xIsMinSize) {
					splitDirection = SplitDirection.Horizontal;
				}
				else {
					splitDirection = SplitDirection.Vertical;
				}
				node.SplitDirection = splitDirection;

				// split box base on direction
				BspNode node1, node2;
				if (splitDirection == SplitDirection.Vertical) {
					// random split between Min boxsize and max box size
					var minRange = (int) (MinPartitionRange * 100);
					var maxRange = (int) (MaxPartitionRange * 100);
					var split = randomInstance.Next(minRange, maxRange) / 100.0;

					var size1 = new Vector2((int) (bbox.Size.X * split), bbox.Size.Y);
					var pos1 = bbox.Position;
					var size2 = new Vector2(bbox.Size.X - size1.X, bbox.Size.Y);
					var pos2 = new Vector2(bbox.Position.X + size1.X, bbox.Position.Y);

					node1 = new BspNode(node, new BoundaryBox(pos1, size1));
					node.AddChild(node1);
					node2 = new BspNode(node, new BoundaryBox(pos2, size2));
					node.AddChild(node2);
				}
				else {
					// random split between Min boxsize and max box size
					var minRange = (int) (MinPartitionRange * 100);
					var maxRange = (int) (MaxPartitionRange * 100);
					var split = randomInstance.Next(minRange, maxRange) / 100.0;

					var size1 = new Vector2(bbox.Size.X, (int) (bbox.Size.Y * split));
					var size2 = new Vector2(bbox.Size.X, bbox.Size.Y - size1.Y);
					var pos1 = bbox.Position;
					var pos2 = new Vector2(bbox.Position.X, bbox.Position.Y + size1.Y);

					node1 = new BspNode(node, new BoundaryBox(pos1, size1));
					node.AddChild(node1);
					node2 = new BspNode(node, new BoundaryBox(pos2, size2));
					node.AddChild(node2);
				}

				// recursive exec for first node
				RecursiveBsp(node1, randomInstance, minimumSideSize);
				node = node2;
			}
		}
		
		/// <summary>
		/// Stochastically selects a direction for splitting.
		/// </summary>
		/// <param name="randomInstance"></param>
		/// <returns>A randomly selected SplitDirection.</returns>
		private static SplitDirection RandomSplitDirection(Random randomInstance)
		{
			var randomDouble = randomInstance.NextDouble();
			if (randomDouble < 0.5) {
				return SplitDirection.Horizontal;
			}
			return SplitDirection.Vertical;
		}
	}
}