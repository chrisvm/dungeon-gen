using System;
using dungeon_gen_lib.Bsp;

namespace dungeon_gen_lib.Room
{
	public class RoomConnector
	{
		public int RoomRadius { get; } 
		
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
		public RoomConnection ConnectRooms(BspNode node)
		{
			var a = node.Children[0];
			var b = node.Children[1];
			
			// calculate the intersection of the two given roomes
			var intersection = CalculateIntersection(node.splitDirection, a.room, b.room);

			// no intersection between the roomes, a z-corridor is needed, also, if there's 
			// some intersection space but not enough to create a axis corridor, create a z-corridor
			var noIntersection = intersection[0] > intersection[1];
			var noAvailableSpaceForIntersection = intersection[1] - intersection[0] < 2 * RoomRadius;
			if (noIntersection || noAvailableSpaceForIntersection) {
				throw new NotImplementedException("Z Corridors are not implemented.");
			}
		
			// create the real working range, taking into 
			// account the constant radius of the room
			var workingRange = new [] {
				intersection[0] + RoomRadius, 
				intersection[1] - RoomRadius
			};
			
			var connection = new RoomConnection {
				Width = RoomRadius,
				SplitDirection = node.splitDirection
			};
			
			// calculate start and end based on the split direction
			var corridorOrigin = Utils.RandomBetween(workingRange[0], workingRange[1], Random);
			
			// calculate the start and end of the connetion based on the calc origin
			if (node.splitDirection == SplitDirection.Vertical) {
				connection.Start = new Vector2(a.room.position.x + a.room.size.x, corridorOrigin);
				connection.End = new Vector2(b.room.position.x, corridorOrigin);
			} else {
				connection.Start = new Vector2(corridorOrigin, a.room.position.y + a.room.size.y);
				connection.End = new Vector2(corridorOrigin, b.room.position.y);
			}
			
			return connection;
		}

		private static double[] CalculateIntersection(SplitDirection splitDirection, BoundaryBox a, BoundaryBox b)
		{
			var intersection = new double[2];
			if (splitDirection == SplitDirection.Vertical) {
				intersection[0] = Math.Max(a.position.y, b.position.y);
				intersection[1] = Math.Min(a.position.y + a.size.y, b.position.y + b.size.y);
			}
			else {
				intersection[0] = Math.Max(a.position.x, b.position.x);
				intersection[1] = Math.Min(a.position.x + a.size.x, b.position.x + b.size.x);
			}
			return intersection;
		}
	}
}