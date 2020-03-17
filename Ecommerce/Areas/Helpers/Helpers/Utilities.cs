using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Areas.Helpers
{
    public class Utilities
    {
        public int Random(int Start,int End)
        {
            Random rnd = new Random();
            int Random = rnd.Next(Start, End);
            return Random;
        }
    }
}
