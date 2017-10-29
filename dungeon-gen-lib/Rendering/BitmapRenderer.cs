using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using dungeon_gen_lib.Bsp;
using dungeon_gen_lib.Room;

namespace dungeon_gen_lib.Rendering
{
	public class BitmapRenderer : IRenderer
	{
		public void Render(BspNode tree, Bitmap bitmap)
		{
			var gfx = Graphics.FromImage(bitmap);
			RecursiveRenderRooms(tree, gfx);
			var leafParents = tree.AllNodesBeneath().Where((node, i) => {
				var childrenAreLeafs = node.Children.Count != 0;
				foreach (var child in node.Children) {
					if (child.Children.Count != 0) childrenAreLeafs = false;
				}
				return childrenAreLeafs;
			}).ToList();
			RenderHighlightedRooms(leafParents, gfx);
		}

		public void Render(IEnumerable<RoomConnection> rooms, Bitmap bitmap)
		{
			throw new System.NotImplementedException();
		}

		private static void RenderHighlightedRooms(IEnumerable<BspNode> nodes, Graphics gfx)
		{
			const int penWidth = 4;
			var pen = new Pen(Color.Azure, penWidth) {
				DashPattern = new [] {1.0f , 1.0f}
			};
			foreach (var node in nodes) {
				gfx.DrawPath(pen, node.bbox.GetGraphicsPath());	
			}
		}

		private static void RecursiveRenderRooms(BspNode nodeTree, Graphics gfx)
		{
			if (nodeTree.room != null) {
				var roomPos = nodeTree.room.position;
				var roomSize = nodeTree.room.size;
				gfx.FillRectangle(Brushes.AliceBlue, 
				                  (float) roomPos.x, 
				                  (float) roomPos.y, 
				                  (float) roomSize.x, 
				                  (float) roomSize.y);
			}
			
			var path = nodeTree.bbox.GetGraphicsPath();
			gfx.DrawPath(Pens.Red, path);
			
			foreach (var child in nodeTree.Children) {
				RecursiveRenderRooms(child, gfx);
			}
		}
		
		/// <summary>
		/// Renders an Image instance to a BitmapImage.
		/// </summary>
		/// <param name="bitmap"></param>
		/// <returns></returns>
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