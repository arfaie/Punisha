using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class MultiModelSearchProduct
    {
        public List<ProductViewModel> ProductViewModels { get; set; }

        public List<category> Categories { get; set; }

        public List<Car> Cars { get; set; }
    }
}
