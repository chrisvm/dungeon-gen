using System;

namespace dungeon_gen_lib
{
	public static class Utils
	{
		/// <summary>
		/// Linear interpolation of a random point inside [a, b).
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="random"></param>
		/// <returns></returns>
		public static double RandomBetween(double a, double b, Random random)
		{
			return a + random.NextDouble() * b;
		}
	}
}