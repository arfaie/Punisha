using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class MultiModelsHome
    {
        public List<Slider> Sliders { get; set; }

        public List<ProductViewModel> ProductViewModels { get; set; }
    }
}
