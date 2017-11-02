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
		
		private Point[] GetPoints()
		{
			var delta = End - Start;
			var perpendicular = delta.Clone();
			perpendicular.Normalize();
			perpendicular.Rotate(Math.PI / 2);
			perpendicular.Scale(Width);
			var points = new [] {
				(Start - perpendicular).AsPoint,
				(End - perpendicular).AsPoint,
				(End + perpendicular).AsPoint,
				(Start + perpendicular).AsPoint
			};
			return points;
		}
		
		public GraphicsPath GetGraphicsPath()
		{
			var path = new GraphicsPath();
			var delta = End - Start;
			
		}
	}
}