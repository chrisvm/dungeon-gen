using System;
using dungeon_gen_lib.Bsp;

namespace dungeon_gen_lib.Room
{
	public class RoomConnector
	{
		public int RoomRadius { get; set; } 
		
		protected readonly Random Random;
		
		/// <summary>
		/// RoomConnector with RoomRadius defaulted to 20.
		/// </summary>
		/// <param name="random"></param>
		public RoomConnector(Random random)
		{
			Random = random;
			RoomRadius = 20;
		}
		
		/// <summary>
		/// Connects the children of the given node with a 
		/// corridor.
		/// </summary>
		/// <param name="node"></param>
		/// <returns>RoomConnection instance of the connecting corridor.</returns>
		/// <exception cref="NotImplementedException"></exception>
		public RoomConnection ConnectRooms(BspNode node)
		{
			var a = node.Children[0];
			var b = node.Children[1];
			
			// calculate the intersection of the two given bboxes
			var intersection = new double[2];
			if (node.splitDirection == SplitDirection.Vertical) {
				intersection[0] = Math.Max(a.bbox.position.y, b.bbox.position.y);
				intersection[1] = Math.Min(a.bbox.position.y + a.bbox.size.y, b.bbox.position.y + b.bbox.size.y);
			} else {
				intersection[0] = Math.Max(a.bbox.position.x, b.bbox.position.x);
				intersection[1] = Math.Min(a.bbox.position.x + a.bbox.size.x, b.bbox.position.x + b.bbox.size.x);
			}
			
			// no intersection between the bboxes,
			// a z-corridor is needed
			if (intersection[0] > intersection[1]) {
				throw new NotImplementedException("Z Corridors are not implemented.");
			}
			
			// if there's some intersection space but not enough to 
			// create a axis corridor, create a z-corridor
			if (intersection[1] - intersection[0] < 2 * RoomRadius) {
				throw new NotImplementedException("Z Corridors are not implemented.");
			}
			
			// create the real working range, taking into 
			// account the constant radius of the room
			var workingRange = new [] {
				intersection[0] + RoomRadius, 
				intersection[1] - RoomRadius
			};
			
			var connection = new RoomConnection {
				Width = RoomRadius
			};
			
			// calculate start and end based on the split direction
			var corridorOrigin = Utils.RandomBetween(workingRange[0], workingRange[1], Random);
			
			// calculate the start and end of the connetion based on the calc origin
			if (node.splitDirection == SplitDirection.Vertical) {
				connection.Start = new Vector2(a.bbox.position.x + a.bbox.size.x, corridorOrigin);
				connection.End = new Vector2(b.bbox.position.x + b.bbox.size.x, corridorOrigin);
			} else {
				connection.Start = new Vector2(corridorOrigin, a.bbox.position.y + a.bbox.size.y);
				connection.End = new Vector2(corridorOrigin, b.bbox.position.y + b.bbox.size.y);
			}
			
			return connection;
		}
	}
}