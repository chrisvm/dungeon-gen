namespace dungeon_gen_lib.Bsp
{
	public class BoundaryBox
	{
		public Vector2 position;
		public Vector2 size;
		
		public BoundaryBox(Vector2 pos, Vector2 size)
		{
			position = pos;
			this.size = size;
		}

		public override string ToString()
		{
			return $"BoundaryBox {{ position={position}, size={size} }}";
		}
	}
}