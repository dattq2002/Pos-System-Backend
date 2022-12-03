using AutoMapper;
using Pos_System.API.Payload.Response.Brands;
using Pos_System.Domain.Models;

namespace Pos_System.API.Mappers.Brands;

public class BrandMapping : Profile
{
    public BrandMapping()
    {
        CreateMap<Brand, CreateNewBrandResponse>();
    }
}