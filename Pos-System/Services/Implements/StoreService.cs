using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
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

    public async Task<IPaginate<GetStoreEmployeesResponse>> GetStoreEmployees(Guid storeId, string? searchUserName, int page, int size)
    {
        if (storeId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Store.EmptyStoreIdMessage);
        IPaginate<GetStoreEmployeesResponse> storeEmployees = new Paginate<GetStoreEmployeesResponse>();
		searchUserName = searchUserName?.Trim().ToLower();

		storeEmployees = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
			selector: x => new GetStoreEmployeesResponse()
			{
				Id = x.Id,
				Name = x.Name,
				Status = EnumUtil.ParseEnum<AccountStatus>(x.Status),
				Role = EnumUtil.ParseEnum<RoleEnum>(x.Role.Name),
				Username = x.Username,
			},
			predicate: string.IsNullOrEmpty(searchUserName) ? 
				x => x.StoreAccount != null && x.StoreAccount.StoreId.Equals(storeId) 
				: x => x.StoreAccount != null && x.StoreAccount.StoreId.Equals(storeId) && x.Username.ToLower().Contains(searchUserName),
			orderBy: x => x.OrderBy(x => x.Username),
			include: x => x.Include(x => x.StoreAccount).Include(x => x.Role),
			page: page,
			size: size);

		return storeEmployees;
    }
}