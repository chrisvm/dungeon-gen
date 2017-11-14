using System;
using dungeon_gen_lib.Bsp;
using dungeon_gen_lib.Room;
using NUnit.Framework;

namespace dungeon_gen_lib.Tests
{
	public class RoomConnectionTests
	{
		private uint Iterations { get; } = 10000;
		
		[Test]
		public void ShouldCorrectlyConnectRoom()
		{
			var random = new Random();
			var roomConnector = new RoomConnector(random);
			var bspNode = TestingTools.CreateVerticallySplitedNode();
			
			TestingTools.IterateAction(Iterations, () => {
				var roomConnection = roomConnector.ConnectRooms(bspNode);
				Console.WriteLine(roomConnection);
			});
		}
		
		[Test]
		public void ShouldCorrectlyCreateEndpointsOnPerpendicularSplittingAxis()
		{
			var random = new Random();
			var roomConnector = new RoomConnector(random);
			var bspNode = TestingTools.CreateVerticallySplitedNode();
			
			var expectedStart = bspNode.Children[0].room.position.x + bspNode.Children[0].room.size.x;
			var expectedEnd = bspNode.Children[1].room.position.x;
			TestingTools.IterateAction(Iterations, () => {
				var roomConnection = roomConnector.ConnectRooms(bspNode);
				Assert.AreEqual(expectedStart, roomConnection.Start.x);
				Assert.AreEqual(expectedEnd, roomConnection.End.x);
			});
		}
	}
}