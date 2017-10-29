using System;
using System.Drawing;

namespace dungeon_gen_lib.Bsp
{
	public class Vector2
	{
		public double x { get; set; }
		public double y { get; set; }
		public Point AsPoint => new Point((int) x, (int) y);
		
		public Vector2() { }

		public Vector2(double x, double y)
		{
			this.x = x;
			this.y = y;
		}
		
		public static Vector2 operator+(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x + b.x, a.y + b.y);
		}

		public static Vector2 operator-(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x - b.x, a.y - b.y);
		}

		public static Vector2 operator *(Vector2 a, double b)
		{
			var ret = a.Clone();
			ret.Scale(b);
			return ret;
		}
		
		public Vector2 Clone()
		{
			return new Vector2(x, y);
		}

		public void Scale(double scale)
		{
			x *= scale;
			y *= scale;
		}
		
		public void Normalize()
		{
			var mag = Magnitude();
			x /= mag;
			y /= mag;
		}

		public double Magnitude()
		{
			return Math.Sqrt(x * x + y * y);
		}
		
		public override string ToString()
		{
			return $"Vector2 {{ X={x}, Y={y} }}";
		}
	}
}