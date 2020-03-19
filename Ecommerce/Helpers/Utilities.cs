using System;

namespace Ecommerce.Helpers
{
	public class Utilities
	{
		public int Random(int Start, int End)
		{
			Random rnd = new Random();
			int Random = rnd.Next(Start, End);
			return Random;
		}
	}
}