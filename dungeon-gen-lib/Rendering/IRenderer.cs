using System.Collections.Generic;
using System.Drawing;
using dungeon_gen_lib.Bsp;
using dungeon_gen_lib.Room;

namespace dungeon_gen_lib.Rendering
{
	public interface IRenderer
	{
		void Render(BspNode tree, Bitmap bitmap);
		void Render(IEnumerable<RoomConnection> rooms, Bitmap bitmap);
	}
}