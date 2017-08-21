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
		public RoomConnection ConnectRooms(BspNode a, BspNode b)
		{
			throw new NotImplementedException();
		}
	}
}