using Pos_System.API.Payload.Request.Brands;
using Pos_System.API.Payload.Response.Brands;

namespace Pos_System.API.Services.Interfaces;

public interface IBrandService
{
    public Task<CreateNewBrandResponse> CreateNewBrand(CreateNewBrandRequest newBrandRequest);
}