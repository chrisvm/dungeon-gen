using System.Drawing;
using System.Drawing.Drawing2D;

namespace dungeon_gen_lib.Bsp
{
	public class BoundaryBox
	{
		public Vector2 position;
		public Vector2 size;
		
		public BoundaryBox(Vector2 pos, Vector2 size)
		{
			position = pos;
			this.size = size;
		}

		public override string ToString()
		{
			return $"BoundaryBox {{ position={position}, size={size} }}";
		}
		
		private static Point[] GetPoints(BoundaryBox bbox)
		{
			var pointA = bbox.position.AsPoint;
			var pointB = new Point((int) (bbox.position.x + bbox.size.x), (int) bbox.position.y);
			var pointC = new Point((int) (bbox.position.x + bbox.size.x), (int) (bbox.position.y + bbox.size.y));
			var pointD = new Point((int) bbox.position.x, (int) (bbox.position.y + bbox.size.y));
			return new [] { pointA, pointB, pointC, pointD };
		}
		
		public GraphicsPath GetGraphicsPath()
		{
			var points = GetPoints(this);
			var gfxPath = new GraphicsPath();
			gfxPath.AddLine(points[0], points[1]);
			gfxPath.AddLine(points[1], points[2]);
			gfxPath.AddLine(points[2], points[3]);
			gfxPath.AddLine(points[3], points[0]);
			return gfxPath;
		}
	}
}