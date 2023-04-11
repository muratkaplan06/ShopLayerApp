using AutoMapper;
using ShopApp.Business.Models;
using ShopApp.DataAccess.Entities;

namespace ShopApp.Business.Mapping
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductModel>()
                .ForMember(x => x.CategoryName, y => y.MapFrom(z => z.Category.Name)).ReverseMap();
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<Product, CrudProductModel>().ReverseMap();
            CreateMap<Product, ProductListModel>()
                .ForMember(x => x.CategoryName, y => y.MapFrom(z => z.Category.Name))
                .ReverseMap();

        }

    }
}
