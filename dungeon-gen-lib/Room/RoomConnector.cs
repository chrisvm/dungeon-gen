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
		
		public const int RoomRadius = 20;
		private Random _random;
		
		public RoomConnector(Random random)
		{
			_random = random;
		}
		
		public RoomConnection ConnectRooms(BspNode node)
		{
			var a = node.Children[0];
			var b = node.Children[1];
			
			// calculate the range of values for posible starting endpoints 
			// depending on the splitting axis of the boundary box
			double axisMax, axisMin;
			if (node.SplitDirection == SplitDirection.Vertical) {
				axisMax = Math.Min(a.Room.Position.Y, b.Room.Position.Y);
				axisMin = Math.Min(a.Room.Position.Y + a.Room.Size.Y, 
				                       b.Room.Position.Y + b.Room.Size.Y);
			} else {
				axisMin = Math.Min(a.Room.Position.X, b.Room.Position.X);
				axisMax = Math.Min(a.Room.Position.X + a.Room.Size.X, 
				                       b.Room.Position.X + b.Room.Size.X);
			}
			
			// create the real working range, taking into 
			// account the constant radius of the room
			var workingRangeMin = axisMin + RoomRadius;
			var workingRangeMax = axisMax + RoomRadius;
			var workingRangeDelta = workingRangeMax - workingRangeMin;
			
			var corridorOrigin = workingRangeMin + _random.NextDouble() * workingRangeDelta; 
			
			// todo: correctly calculate the start and end of the connetion based on the calc origin
			
			return new RoomConnection {
				Width = RoomRadius
			};
		}
	}
}