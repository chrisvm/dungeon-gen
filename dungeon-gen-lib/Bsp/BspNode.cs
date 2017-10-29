using System.Collections.Generic;

namespace dungeon_gen_lib.Bsp
{
	public class BspNode
	{
		public BspNode Parent { get; set; }
		public IList<BspNode> Children { get; set; }
		public BoundaryBox bbox { get; set; }
		public BoundaryBox room { get; set; }
		public SplitDirection splitDirection { get; set; }

		public BspNode(BoundaryBox bbox)
		{
			Children = new List<BspNode>();
			Parent = null;
			this.bbox = bbox;
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