using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using dungeon_gen_lib.Bsp;

namespace dungeon_gen_lib.Rendering
{
	public class BitmapRenderer : IRenderer
	{
		public Bitmap Render(BspNode tree)
		{
			var bitmap = new Bitmap((int) tree.bbox.size.x, (int) tree.bbox.size.y);
			RecursiveRenderToBitmap(tree, bitmap);
			return bitmap;
		}
		
		private static void RecursiveRenderToBitmap(BspNode nodeTree, Image bitmap, Graphics gfx = null)
		{
			if (gfx == null) {
				gfx = Graphics.FromImage(bitmap);
				gfx.FillRectangle(Brushes.Black, 0, 0, bitmap.Width, bitmap.Height);
			}
			
			if (nodeTree.room != null) {
				var roomPos = nodeTree.room.position;
				var roomSize = nodeTree.room.size;
				gfx.FillRectangle(Brushes.AliceBlue, 
				                  (float) roomPos.x, 
				                  (float) roomPos.y, 
				                  (float) roomSize.x, 
				                  (float) roomSize.y);
			}
			
			var points = GetPointsFromBBox(nodeTree.bbox);
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
			var pointA = bbox.position.AsPoint;
			var pointB = new Point((int) (bbox.position.x + bbox.size.x), (int) bbox.position.y);
			var pointC = new Point((int) (bbox.position.x + bbox.size.x), (int) (bbox.position.y + bbox.size.y));
			var pointD = new Point((int) bbox.position.x, (int) (bbox.position.y + bbox.size.y));
			return new [] { pointA, pointB, pointC, pointD };
		}
		
		public static BitmapImage BitmapToImageSource(Image bitmap)
		{
			using (var memory = new MemoryStream())
			{
				bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
				memory.Position = 0;
				var bitmapimage = new BitmapImage();
				bitmapimage.BeginInit();
				bitmapimage.StreamSource = memory;
				bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapimage.EndInit();
				return bitmapimage;
			}
		}
	}
}