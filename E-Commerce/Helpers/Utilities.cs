using System;

namespace ECommerce.Helpers
{
	public class Utilities
	{
		public int Random(int start, int end)
		{
			Random rnd = new Random();
			int random = rnd.Next(start, end);
			return random;
		}
	}
}