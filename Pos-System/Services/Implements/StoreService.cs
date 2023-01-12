using AutoMapper;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Stores;
using Pos_System.API.Payload.Response.Stores;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Domain.Paginate;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements;

public class StoreService : BaseService<StoreService>, IStoreService
{
    public StoreService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<StoreService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
    {
    }

    public async Task<IPaginate<GetStoreResponse>> GetStoresInBrand(Guid brandId, string? searchShortName, int page, int size)
    {
        if (brandId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.EmptyBrandIdMessage);
        searchShortName = searchShortName?.Trim().ToLower();
        IPaginate<GetStoreResponse> storesInBrandResponse = await _unitOfWork.GetRepository<Store>().GetPagingListAsync(
            selector: x => new GetStoreResponse(x.Id, x.BrandId, x.Name, x.ShortName, x.Email, x.Address, x.Status),
            predicate: string.IsNullOrEmpty(searchShortName) ? x => x.BrandId.Equals(brandId) : x => x.BrandId.Equals(brandId) && x.ShortName.ToLower().Contains(searchShortName),
            orderBy: x => x.OrderBy(x => x.ShortName),
            page: page,
            size: size
        );
        return storesInBrandResponse;
    }

    public async Task<GetStoreDetailResponse> GetStoreById(Guid storeId)
    {
        if (storeId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Store.EmptyStoreIdMessage);
        GetStoreDetailResponse storeDetailResponse = await _unitOfWork.GetRepository<Store>().SingleOrDefaultAsync(
            selector: x => new GetStoreDetailResponse(x.Id, x.BrandId, x.Name, x.Name, x.Email, x.Address, x.Status, x.Phone, x.Code),
            predicate: x => x.Id.Equals(storeId)
            );
        return storeDetailResponse;
    }

    public async Task<CreateNewStoreResponse> CreateNewStore(CreateNewStoreRequest newStoreRequest)
    {
        _logger.LogInformation($"Create new store with {newStoreRequest.Name}");
        Store newStore = _mapper.Map<Store>(newStoreRequest);
        newStore.Status = StoreStatus.Active.GetDescriptionFromEnum();
        newStore.Id = Guid.NewGuid();
        await _unitOfWork.GetRepository<Store>().InsertAsync(newStore);
        bool isSuccessfull = await _unitOfWork.CommitAsync() > 0;
        CreateNewStoreResponse createNewStoreResponse = null;
        if (isSuccessfull)
        {
            createNewStoreResponse = _mapper.Map<CreateNewStoreResponse>(newStore);
        }
        return createNewStoreResponse;
    }
}