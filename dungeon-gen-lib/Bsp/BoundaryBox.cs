namespace dungeon_gen_lib.Bsp
{
	public class BoundaryBox
	{
		public Vector2 Position;
		public Vector2 Size;
		
		public BoundaryBox(Vector2 pos, Vector2 size)
		{
			Position = pos;
			Size = size;
		}

		public override string ToString()
		{
			return $"BoundaryBox {{ Position={Position}, Size={Size} }}";
		}
	}
}