using System;

namespace ECommerce.Helpers
{
	public class Utilities
	{
		public int Random(int start, int end)
		{
			var rnd = new Random();
			var random = rnd.Next(start, end);
			return random;
		}
	}
}