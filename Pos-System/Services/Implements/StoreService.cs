using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Stores;
using Pos_System.API.Payload.Response.Menus;
using Pos_System.API.Payload.Response.Stores;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Domain.Paginate;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements;

public class StoreService : BaseService<StoreService>, IStoreService
{
    private readonly IMenuService _menuService;
    public StoreService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<StoreService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IMenuService menuService) : base(unitOfWork, logger, mapper, httpContextAccessor)
    {
        _menuService = menuService;
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
            selector: x => new GetStoreDetailResponse(x.Id, x.BrandId, x.Name, x.Name, x.Email, x.Address, x.Status,
                x.Phone, x.Code, x.Brand.PicUrl),
            include: x => x.Include(x => x.Brand),
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
        HasBaseMenuResponse hasBaseMenu = await _menuService.CheckHasBaseMenuInBrand(newStoreRequest.BrandId);
        if (hasBaseMenu.HasBaseMenu)
        {
            Menu brandBaseMenu = await _unitOfWork.GetRepository<Menu>().SingleOrDefaultAsync(
                predicate: menu => menu.BrandId.Equals(newStoreRequest.BrandId) && menu.Priority == 0
            );
            MenuStore newMenuStore = new MenuStore()
            {
                Id = Guid.NewGuid(),
                MenuId = brandBaseMenu.Id,
                StoreId = newStore.Id
            };

            List<MenuStore> newMenuStores = new List<MenuStore>
            {
                newMenuStore
            };
            newStore.MenuStores = newMenuStores;
        }
        await _unitOfWork.GetRepository<Store>().InsertAsync(newStore);
        bool isSuccessfull = await _unitOfWork.CommitAsync() > 0;
        CreateNewStoreResponse createNewStoreResponse = null;
        if (isSuccessfull)
        {
            createNewStoreResponse = _mapper.Map<CreateNewStoreResponse>(newStore);
        }
        return createNewStoreResponse;
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

    public async Task<bool> UpdateStoreInformation(Guid storeId, UpdateStoreRequest updateStoreRequest)
    {
        if (storeId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Store.EmptyStoreIdMessage);
        Store storeForUpdate = await _unitOfWork.GetRepository<Store>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(storeId));
        if (storeForUpdate == null) throw new BadHttpRequestException(MessageConstant.Store.StoreNotFoundMessage);
        _logger.LogInformation($"Start update store {storeId}");

        updateStoreRequest.TrimString();
        storeForUpdate.Name = string.IsNullOrEmpty(updateStoreRequest.Name) ? storeForUpdate.Name : updateStoreRequest.Name;
        storeForUpdate.ShortName = string.IsNullOrEmpty(updateStoreRequest.ShortName) ? storeForUpdate.ShortName : updateStoreRequest.ShortName;
        storeForUpdate.Email = string.IsNullOrEmpty(updateStoreRequest.Email) ? storeForUpdate.Email : updateStoreRequest.Email;
        storeForUpdate.Phone = string.IsNullOrEmpty(updateStoreRequest.Phone) ? storeForUpdate.Phone : updateStoreRequest.Phone;
        storeForUpdate.Code = string.IsNullOrEmpty(updateStoreRequest.Code) ? storeForUpdate.Code : updateStoreRequest.Code;
        storeForUpdate.Address = string.IsNullOrEmpty(updateStoreRequest.Address) ? storeForUpdate.Address : updateStoreRequest.Address;

        _unitOfWork.GetRepository<Store>().UpdateAsync(storeForUpdate);
        bool isSuccesful = await _unitOfWork.CommitAsync() > 0;

        return isSuccesful;
    }

    public async Task<GetMenuDetailForStaffResponse> GetMenuDetailForStaff()
    {
        Guid userStoreId = Guid.Parse(GetStoreIdFromJwt());
        if (userStoreId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Menu.MissingStoreIdMessage);

        Guid menuOfStoreId = await _unitOfWork.GetRepository<MenuStore>()
            .SingleOrDefaultAsync(selector: x => x.MenuId, predicate: x => x.StoreId.Equals(userStoreId));

        Guid userBrandId = await _unitOfWork.GetRepository<Store>()
            .SingleOrDefaultAsync(selector: x => x.BrandId, predicate: x => x.Id.Equals(userStoreId));

        GetMenuDetailForStaffResponse menuOfStore = await _unitOfWork.GetRepository<Menu>().SingleOrDefaultAsync(selector: x => new GetMenuDetailForStaffResponse(
            x.Id,
            x.BrandId,
            x.Code,
            x.Priority,
            true,
            x.DateFilter,
            x.StartTime,
            x.EndTime),
            predicate: x => x.Id.Equals(menuOfStoreId) && x.Status.Equals(MenuStatus.Active.GetDescriptionFromEnum()));

        menuOfStore.ProductsInMenu = (List<ProductDataForStaff>)await _unitOfWork.GetRepository<MenuProduct>().GetListAsync(
            selector: x => new ProductDataForStaff
            (
                x.ProductId,
                x.Product.Code,
                x.Product.Name,
                x.SellingPrice,
                x.Product.PicUrl,
                x.Product.Status,
                x.HistoricalPrice,
                x.DiscountPrice,
                x.Product.Description,
                x.Product.DisplayOrder,
                x.Product.Size,
                x.Product.Type,
                x.Product.ParentProductId,
                x.Product.BrandId,
                x.Product.CategoryId,
                (List<Guid>)x.Product.CollectionProducts.Select(x => x.CollectionId),
                (List<Guid>)x.Product.Category.ExtraCategoryProductCategories.Select(x => x.ExtraCategoryId),
                x.Id
            ),
            predicate: x => x.MenuId.Equals(menuOfStoreId),
            include: menuProduct => menuProduct
                .Include(menuProduct => menuProduct.Product)
                .ThenInclude(product => product.CollectionProducts)
                .Include(menuProduct => menuProduct.Product)
                .ThenInclude(product => product.Category)
                .ThenInclude(category => category.ExtraCategoryProductCategories)
            );

        menuOfStore.CollectionsOfBrand = (List<CollectionOfBrand>)await _unitOfWork.GetRepository<Collection>()
            .GetListAsync(selector: x => new CollectionOfBrand(
                x.Id,
                x.Name,
                x.Code,
                x.PicUrl,
                x.Description
                ), predicate: x => x.BrandId.Equals(userBrandId) && x.Status == CollectionStatus.Active.GetDescriptionFromEnum());

        menuOfStore.CategoriesOfBrand = (List<CategoryOfBrand>)await _unitOfWork.GetRepository<Category>()
            .GetListAsync(selector: x => new CategoryOfBrand(
                x.Id,
                x.Code,
                x.Name,
                EnumUtil.ParseEnum<CategoryType>(x.Type),
                x.DisplayOrder,
                x.Description,
                x.PicUrl
                ), predicate: x => x.BrandId.Equals(userBrandId));

        return menuOfStore;
    }
}