using AutoMapper;
using Pos_System.API.Enums;
using Pos_System.API.Models.Response.Menu;
using Pos_System.API.Models.Response.Product;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;

namespace Pos_System.API.Mappers;

public class MappingProfiles : Profile
{
	public MappingProfiles()
	{
		CreateMap<Menu, MenuResponse>()
			.ForMember(x => x.Id, src => src.MapFrom(x => x.Id))
			.ForMember(x => x.Code, src => src.MapFrom(src => src.Code))
			.ForMember(x => x.Products, src => src.Ignore());
		CreateMap<MenuProduct, ProductInMenuResponse>()
			.ForMember(x => x.Code, src => src.MapFrom(src => src.Product.Code))
			.ForMember(x => x.Id, src => src.MapFrom(src => src.Product.Id))
			.ForMember(x => x.Name, src => src.MapFrom(src => src.Product.Name))
			.ForMember(x => x.DisplayOrder, src => src.MapFrom(src => src.Product.DisplayOrder))
			.ForMember(x => x.Type, src => src.MapFrom(src => EnumUtil.ParseEnum<ProductType>(src.Product.Type)))
			.ForMember(x => x.ExtraCategoryCode, src => src.MapFrom(src => src.Product.CategoryCodeNavigation.ExtraCategoryExtraCategoryCodeNavigations.Select(x => x.ExtraCategoryCode)))
			.ForMember(x => x.SellingPrice, src => src.MapFrom(src => src.SellingPrice))
			.ForMember(x => x.DiscountPrice, src => src.MapFrom(src => src.DiscountPrice))
			.ForMember(x => x.HistoricalPrice, src => src.MapFrom(src => src.HistoricalPrice));
	}
}