using System;
using dungeon_gen_lib.Bsp;
using dungeon_gen_lib.Room;

namespace dungeon_gen_lib.Tests
{
	/// <summary>
	/// Collection of methods used in unit tests.
	/// </summary>
	public static class TestingTools
	{
		/// <summary>
		/// Iterates a given action a number of iterations.
		/// </summary>
		/// <param name="iterations"></param>
		/// <param name="action"></param>
		public static void IterateAction(uint iterations, Action action)
		{
			for (var iteration = 0; iteration < iterations; iteration++) {
				action();
			}
		}
		
		/// <summary>
		/// Creates a vertically splited node with BoundaryBox of size 100, 100.
		/// </summary>
		/// <returns></returns>
		public static BspNode CreateVerticallySplitedNode()
		{
			var boundaryBox = new BoundaryBox(new Vector2(0, 0), new Vector2(100, 100));
			var bspNode = new BspNode(boundaryBox);
			
			var aBoundaryBox = new BoundaryBox(new Vector2(0, 0), new Vector2(50, 100));
			var a = new BspNode(aBoundaryBox) {
				room = new BoundaryBox {
					position = new Vector2(12.5, 25),
					size = new Vector2(25, 50)
				}
			};

			var bBoundaryBox = new BoundaryBox(new Vector2(50, 0), new Vector2(50, 100));
			var b = new BspNode(bBoundaryBox) {
				room = new BoundaryBox {
					position = new Vector2(62.5, 25),
					size = new Vector2(25, 50)
				}
			};

			bspNode.AddChild(a);
			bspNode.AddChild(b);
			bspNode.splitDirection = SplitDirection.Vertical;
			
			return bspNode;
		}
	}
	
	/// <summary>
	/// Expose to expose private methods in <see cref="RoomCreator"/> class.
	/// </summary>
	public class RoomCreatorExpose : RoomCreator
	{
		public RoomCreatorExpose(Random random) : base(random) {}
            
		public BoundaryBox CreateRoom(BoundaryBox bbox)
		{
			return _CreateRoom(bbox, Random);
		}
	}
}