using dungeon_gen_lib.Bsp;
using dungeon_gen_lib.Room;
using NUnit.Framework;

namespace dungeon_gen_lib.Tests
{
    public class RoomCreationTests
    {
        private const uint Iterations = 10000;
        
        private class RoomCreatorExpose : RoomCreator
        {
            public new static BoundaryBox CreateRoom(BoundaryBox bbox)
            {
                return RoomCreator.CreateRoom(bbox);
            }
        }

        [Test]
        public void CreatesRoomInsideBoundary()
        {
            var bbox = new BoundaryBox(new Vector2(), new Vector2(100, 100));

            for (var iteration = 0; iteration < Iterations; iteration++) {
                var room = RoomCreatorExpose.CreateRoom(bbox);
            
                Assert.Less(bbox.Position.X, room.Position.X);
                Assert.Less(bbox.Position.Y, room.Position.Y);
                Assert.Greater(bbox.Size.X, room.Size.X);
                Assert.Greater(bbox.Size.Y, room.Size.Y);
                Assert.Greater(bbox.Size.X, room.Position.X + room.Size.X);
                Assert.Greater(bbox.Size.Y, room.Position.Y + room.Size.Y);
            }
        }
    }
}