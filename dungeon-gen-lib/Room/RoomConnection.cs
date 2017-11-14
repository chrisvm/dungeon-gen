using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.NetworkInformation;
using dungeon_gen_lib.Bsp;

namespace dungeon_gen_lib.Room
{
	public class RoomConnection
	{
		public Vector2 Start { get; set; }	
		public Vector2 End { get; set; }
		public SplitDirection SplitDirection { get; set; }
		public int Width { get; set; }
		
		private static Point[] GetPoints(RoomConnection roomConnection)
		{
			var delta = roomConnection.End - roomConnection.Start;
			var perpendicular = delta.Clone();
			perpendicular.Normalize();
			perpendicular.Rotate(Math.PI / 2);
			perpendicular.Scale(roomConnection.Width);
			var points = new [] {
				(roomConnection.Start - perpendicular).AsPoint,
				(roomConnection.End - perpendicular).AsPoint,
				(roomConnection.End + perpendicular).AsPoint,
				(roomConnection.Start + perpendicular).AsPoint
			};
			return points;
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
		
		public override string ToString()
		{
			var ret = $"RoomConnection {{ Start={Start}, End={End}, Width={Width} }}";
			return ret;			
		}
	}
}