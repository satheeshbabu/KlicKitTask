using System.Linq;
using AutoMapper;
using KlicKitApi.Dtos;
using KlicKitApi.Models;

namespace KlicKitApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {            
            CreateMap<User, UserForDetailedDto>();                         
            CreateMap<UserForRegisterDto, User>();

            CreateMap<Product, ProductForDetailedDto>(); 
            CreateMap<Product, ProductForListDto>(); 
            
            CreateMap<UserProducts, UserProductsForListDto>(); 
            CreateMap<UserProducts, UserProductsForListDto>(); 

        }
    }
}