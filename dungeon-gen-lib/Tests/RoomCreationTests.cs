using System;
using dungeon_gen_lib.Bsp;
using dungeon_gen_lib.Room;
using NUnit.Framework;

namespace dungeon_gen_lib.Tests
{
    public class RoomCreationTests
    {
        private const uint Iterations = 10000;
        
        [Test]
        public void CreatesRoomInsideBoundary()
        {
            var bbox = new BoundaryBox(new Vector2(), new Vector2(100, 100));
            var random = new Random();
            var roomCreator = new RoomCreatorExpose(random);
            
            for (var iteration = 0; iteration < Iterations; iteration++) {
                var room = roomCreator.CreateRoom(bbox);
            
                Assert.Less(bbox.Position.X, room.Position.X);
                Assert.Less(bbox.Position.Y, room.Position.Y);
                Assert.Greater(bbox.Size.X, room.Size.X);
                Assert.Greater(bbox.Size.Y, room.Size.Y);
                Assert.Greater(bbox.Size.X, room.Position.X + room.Size.X);
                Assert.Greater(bbox.Size.Y, room.Position.Y + room.Size.Y);
            }
        }
        
        public class RoomCreatorExpose : RoomCreator
        {
            public RoomCreatorExpose(Random random) : base(random) {}
            
            public BoundaryBox CreateRoom(BoundaryBox bbox)
            {
                return _CreateRoom(bbox, _random);
            }
        }
    }
}