using AutoMapper;
using IcartE.Data;
using IcartE1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<RegisterViewModel, Customer>();
        }
    }
}
