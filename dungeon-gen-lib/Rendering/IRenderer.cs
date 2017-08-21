using System.Drawing;
using dungeon_gen_lib.Bsp;

namespace dungeon_gen_lib.Rendering
{
	public interface IRenderer
	{
		Bitmap Render(BspNode tree);
	}
}