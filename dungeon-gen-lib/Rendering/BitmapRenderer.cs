using System.Drawing;
using dungeon_gen_lib.Bsp;

namespace dungeon_gen_lib.Rendering
{
	public class BitmapRenderer : IRenderer
	{
		private static BitmapRenderer _instance;
		public static BitmapRenderer Instance => _instance ?? (_instance = new BitmapRenderer());

		public Bitmap Render(BspNode tree)
		{
			var bitmap = new Bitmap((int) tree.BBox.Size.X, (int) tree.BBox.Size.Y);
			RecursiveRenderToBitmap(tree, bitmap);
			return bitmap;
		}
		
		private static void RecursiveRenderToBitmap(BspNode nodeTree, Image bitmap, Graphics gfx = null)
		{
			if (gfx == null) {
				gfx = Graphics.FromImage(bitmap);
				gfx.FillRectangle(Brushes.Black, 0, 0, bitmap.Width, bitmap.Height);
			}
			
			if (nodeTree.Room != null) {
				var roomPos = nodeTree.Room.Position;
				var roomSize = nodeTree.Room.Size;
				gfx.FillRectangle(Brushes.AliceBlue, 
				                  (float) roomPos.X, 
				                  (float) roomPos.Y, 
				                  (float) roomSize.X, 
				                  (float) roomSize.Y);
			}
			
			var points = GetPointsFromBBox(nodeTree.BBox);
			gfx.DrawLine(Pens.Red, points[0], points[1]);
			gfx.DrawLine(Pens.Red, points[1], points[2]);
			gfx.DrawLine(Pens.Red, points[2], points[3]);
			gfx.DrawLine(Pens.Red, points[3], points[0]);
			
			foreach (var child in nodeTree.Children) {
				RecursiveRenderToBitmap(child, bitmap, gfx);
			}
		}

		private static Point[] GetPointsFromBBox(BoundaryBox bbox)
		{
			var pointA = bbox.Position.AsPoint;
			var pointB = new Point((int) (bbox.Position.X + bbox.Size.X), (int) bbox.Position.Y);
			var pointC = new Point((int) (bbox.Position.X + bbox.Size.X), (int) (bbox.Position.Y + bbox.Size.Y));
			var pointD = new Point((int) bbox.Position.X, (int) (bbox.Position.Y + bbox.Size.Y));
			return new [] { pointA, pointB, pointC, pointD };
		}
	}
}