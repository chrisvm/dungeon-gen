using System;
using dungeon_gen_lib.Bsp;

namespace dungeon_gen_lib.Room
{
	public class RoomConnection
	{
		public Vector2 Start { get; set; }	
		public Vector2 End { get; set; }	
		public int Width { get; set; }
	}
	
	public class RoomConnector
	{
		public RoomConnection ConnectRooms(BspNode node)
		{
			var a = node.Children[0];
			var b = node.Children[1];
			if (node.SplitDirection == SplitDirection.Vertical) {
				var axisMin = Math.Min(a.Room.Position.Y, b.Room.Position.Y);
				var axisMax = Math.Min(a.Room.Position.Y + a.Room.Size.Y, 
				                       b.Room.Position.Y + b.Room.Size.Y);
			} else {
				var axisMin = Math.Min(a.Room.Position.X, b.Room.Position.X);
				var axisMax = Math.Min(a.Room.Position.X + a.Room.Size.X, 
				                       b.Room.Position.X + b.Room.Size.X);
			}
		}
	}
}