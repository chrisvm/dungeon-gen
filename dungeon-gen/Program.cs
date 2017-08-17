using System;
using System.Drawing;
using dungeon_gen.Bsp;

namespace dungeon_gen
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var mapGenerator = new BinarySpacePartition {
				MinimumSideSize = 100,
				PrintDebug = false
			};
			
			// bsp partition
			var bbox = new BoundaryBox(new Vector2(0, 0), new Vector2(800, 800));
			var nodeTree = mapGenerator.Partition(bbox);
			Console.WriteLine($"Tree.Children.Count = {nodeTree.Children.Count}");
			
			// create rooms
			RoomCreator.CreateRooms(nodeTree);
			
			// render partition
			PrintToBitmap(nodeTree);
		}
		
		private static void PrintToBitmap(BspNode nodeTree)
		{
			var bitmap = new Bitmap((int) nodeTree.BBox.Size.X, (int) nodeTree.BBox.Size.Y);
			RenderToBitmap(nodeTree, bitmap);
			var path = System.IO.Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				"Example.png");
			bitmap.Save(path);
		}

		private static void RenderToBitmap(BspNode nodeTree, Image bitmap, Graphics gfx = null)
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
				RenderToBitmap(child, bitmap, gfx);
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