using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class SMSViewModel
    {
        public class Return
        {
            public int status { get; set; }
            public string message { get; set; }
        }

        public class Entry
        {
            public int messageid { get; set; }
            public string message { get; set; }
            public int status { get; set; }
            public string statustext { get; set; }
            public string sender { get; set; }
            public string receptor { get; set; }
            public int date { get; set; }
            public int cost { get; set; }
        }

        public class RootObject
        {
            public Return @return { get; set; }
            public List<Entry> entries { get; set; }
        }

    }
}
