using AutoMapper;
using IcartE1.Data;
using IcartE1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<BaseProductViewModel, Product>().ReverseMap();
        }
    }
}
