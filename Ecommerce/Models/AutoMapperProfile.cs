using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Models.ViewModels;

namespace Ecommerce.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddEditAgencyViewModel, Agency>();
            CreateMap<AddEditProductViewModel, Product>();
            CreateMap<AddEditProductGallery, ProductGallery>();
            CreateMap<AddEditSelectItemViewModel, SelectItem>();
            CreateMap<AddEditFieldViewModel, Field>();
            CreateMap<AddEditCityViewModel, City>();
        }
    }
}
