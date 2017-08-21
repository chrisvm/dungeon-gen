using System;
using System.Drawing;

namespace dungeon_gen.Bsp
{
	public class Vector2
	{
		public double X { get; set; }
		public double Y { get; set; }
		public Point AsPoint => new Point((int) X, (int) Y);
		
		public Vector2() { }

		public Vector2(double x, double y)
		{
			X = x;
			Y = y;
		}
		
		public static Vector2 operator+(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X + b.X, a.Y + b.Y);
		}

		public static Vector2 operator-(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X - b.X, a.Y - b.Y);
		}

		public static Vector2 operator *(Vector2 a, double b)
		{
			var ret = a.Clone();
			ret.Scale(b);
			return ret;
		}
		
		public Vector2 Clone()
		{
			return new Vector2(X, Y);
		}

		public void Scale(double scale)
		{
			X *= scale;
			Y *= scale;
		}
		
		public void Normalize()
		{
			var mag = Magnitude();
			X /= mag;
			Y /= mag;
		}

		public double Magnitude()
		{
			return Math.Sqrt(X * X + Y * Y);
		}
		
		public override string ToString()
		{
			return $"Vector2 {{ X={X}, Y={Y} }}";
		}
	}
}