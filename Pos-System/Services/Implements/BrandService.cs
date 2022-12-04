using AutoMapper;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Brands;
using Pos_System.API.Payload.Response.Brands;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements;

public class BrandService : BaseService<BrandService>, IBrandService
{
    public BrandService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<BrandService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
    {
    }

    public async Task<CreateNewBrandResponse> CreateNewBrand(CreateNewBrandRequest newBrandRequest)
    {
        _logger.LogInformation($"Create new brand with {newBrandRequest.Name}");
        Brand newBrand = _mapper.Map<Brand>(newBrandRequest);
        newBrand.Status = BrandStatus.Active.GetDescriptionFromEnum();
        newBrand.Id = Guid.NewGuid();
        await _unitOfWork.GetRepository<Brand>().InsertAsync(newBrand);
        bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
        CreateNewBrandResponse createNewBrandResponse = null;
        if (isSuccessful)
        {
            createNewBrandResponse = _mapper.Map<CreateNewBrandResponse>(newBrand);
        }
        return createNewBrandResponse;
    }
}