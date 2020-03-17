using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class MultiModelsProductDetailes
    {
        public List<ProductViewModel> ProductViewModels { get; set; }

     

        public IEnumerable<ProductFieldViewModel> ProductFieldViewModels { get; set; }
    }
}
