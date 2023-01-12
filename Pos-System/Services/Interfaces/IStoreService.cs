using Pos_System.API.Payload.Response.Stores;
using Pos_System.Domain.Paginate;

namespace Pos_System.API.Services.Interfaces;

public interface IStoreService
{
    public Task<IPaginate<GetStoreResponse>> GetStoresInBrand(Guid brandId, string? searchShortName, int page, int size);

    public Task<GetStoreDetailResponse> GetStoreById(Guid storeId);

    public Task<IPaginate<GetStoreEmployeesResponse>> GetStoreEmployees(Guid storeId, string? searchUserName, int page, int size);
}