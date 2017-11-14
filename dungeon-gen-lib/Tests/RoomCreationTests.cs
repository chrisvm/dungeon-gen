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
            
            TestingTools.IterateAction(Iterations, () => {
                var room = roomCreator.CreateRoom(bbox);
                Assert.Less(bbox.position.x, room.position.x);
                Assert.Less(bbox.position.y, room.position.y);
                Assert.Greater(bbox.size.x, room.size.x);
                Assert.Greater(bbox.size.y, room.size.y);
                Assert.Greater(bbox.size.x, room.position.x + room.size.x);
                Assert.Greater(bbox.size.y, room.position.y + room.size.y); 
            });
        }
    }
}