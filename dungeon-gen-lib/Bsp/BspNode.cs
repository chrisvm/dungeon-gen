using System.Collections.Generic;

namespace dungeon_gen_lib.Bsp
{
	public class BspNode
	{
		public BspNode Parent { get; set; }
		public IList<BspNode> Children { get; set; }
		public BoundaryBox BBox { get; set; }
		public BoundaryBox Room { get; set; }
		public SplitDirection SplitDirection { get; set; }

		public BspNode(BoundaryBox bbox)
		{
			Children = new List<BspNode>();
			Parent = null;
			BBox = bbox;
		}
		
		public BspNode(BspNode parent, BoundaryBox bbox)
			: this(bbox)
		{
			Parent = parent;
		}

		public void AddChild(BspNode child)
		{
			child.Parent = this;
			Children.Add(child);
		}
	}
}