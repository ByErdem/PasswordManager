using AutoMapper;
using PasswordManager.Data;
using PasswordManager.Entity.Concrete;
using PasswordManager.Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PasswordManager.Mvc.Models.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<USER, MUser>().ReverseMap();
            CreateMap<MUser, USER>().ReverseMap();

            CreateMap<PRODUCT, ProductDto>().ReverseMap();
            CreateMap<ProductDto, PRODUCT>().ReverseMap();

            CreateMap<CATEGORY, MCategory>().ReverseMap();
            CreateMap<MCategory, CATEGORY>().ReverseMap();

            CreateMap<CATEGORY, CategoryDto>().ReverseMap();
            CreateMap<CategoryDto, CATEGORY>().ReverseMap();

            CreateMap<List<CATEGORY>, List<MCategory>>().ReverseMap();
            CreateMap<List<MCategory>, List<CATEGORY>>().ReverseMap();
        }
    }
}