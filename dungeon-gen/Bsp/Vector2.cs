using System;
using System.Drawing;

namespace dungeon_gen.Bsp
{
	public class Vector2<T>
	{
		public T X;
		public T Y;
		
		public Vector2() { }
		
		public Vector2(T x, T y)
		{
			X = x;
			Y = y;
		}

		public Vector2<T> Clone()
		{
			return new Vector2<T>(X, Y);
		}
		
		public override string ToString()
		{
			return $"Vector2 {{ X={X}, Y={Y} }}";
		}
	}

	public class Vector2 : Vector2<double>
	{
		public Point AsPoint => new Point((int) X, (int) Y);
		
		public Vector2() { }
		public Vector2(double x, double y) : base(x, y) {}
		
		public static Vector2 operator+(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X + b.X, a.Y + b.Y);
		}

		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X - b.X, a.Y - b.Y);
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
	}
}