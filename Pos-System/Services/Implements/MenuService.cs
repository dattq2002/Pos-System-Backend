using AutoMapper;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Menus;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements
{
    public class MenuService : BaseService<MenuService>, IMenuService
    {
        public MenuService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<MenuService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewMenu(CreateNewMenuRequest createNewMenuRequest)
        {
	        _logger.LogInformation($"Create new Menu with menu code: {createNewMenuRequest.Code}");
	        Menu newMenu = new Menu()
	        {
		        Id = Guid.NewGuid(),
		        Code = createNewMenuRequest.Code.Trim(),
                MenuStores = new List<MenuStore>(),
                MenuProducts = new List<MenuProduct>(),
                CreatedBy = GetUsernameFromJwt(),
                CreatedAt = DateTime.UtcNow,
	        };
	        if (createNewMenuRequest.ProductsInMenu.Any())
	        {
		        foreach (var productInMenu in createNewMenuRequest.ProductsInMenu)
		        {
			        Product product = await _unitOfWork.GetRepository<Product>()
				        .SingleOrDefaultAsync(predicate: x => x.Id.Equals(productInMenu.Id));
			        if (product == null)
			        {
                        _logger.LogInformation($"Product is not found {productInMenu.Id}");
                        throw new BadHttpRequestException(MessageConstant.Product.ProductNotFoundMessage);
			        }
			        newMenu.MenuProducts.Add(new MenuProduct()
			        {
				        Id = Guid.NewGuid(),
				        ProductId = product.Id,
                        MenuId = newMenu.Id,
                        DiscountPrice = productInMenu.DiscountPrice.HasValue ?  productInMenu.DiscountPrice.Value : 0,
						SellingPrice = productInMenu.SellingPrice != product.SellingPrice ? productInMenu.SellingPrice : product.SellingPrice,
                        HistoricalPrice = product.HistoricalPrice,
                        Status = EnumUtil.GetDescriptionFromEnum(MenuProductStatus.Active),
                        CreatedBy = GetUsernameFromJwt(),
                        CreatedAt = DateTime.UtcNow,
			        });
                }
	        };
	        if (createNewMenuRequest.StoresInMenu.Any())
	        {
		        foreach (var storeInMenu in createNewMenuRequest.StoresInMenu)
		        {
			        Store store = await _unitOfWork.GetRepository<Store>()
				        .SingleOrDefaultAsync(predicate: x => x.Id.Equals(storeInMenu.Id));
			        if (store == null)
			        {
				        _logger.LogInformation($"Store is not found {storeInMenu.Id}");
				        throw new BadHttpRequestException(MessageConstant.Store.StoreNotFoundMessage);
			        }
                    newMenu.MenuStores.Add(new MenuStore()
                    {
                        Id = Guid.NewGuid(),
                        MenuId = newMenu.Id,
                        StoreId = store.Id,
                        Status = EnumUtil.GetDescriptionFromEnum(MenuProductStatus.Deactivate),
                        Priority = storeInMenu.Priority,
                        DateFilter = storeInMenu.DateFilter,
                        StartTime = storeInMenu.StartTime,
                        EndTime = storeInMenu.EndTime,
                    });
		        }
            };
	        await _unitOfWork.GetRepository<Menu>().InsertAsync(newMenu);
	        bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;
	        if (isSuccessfully)
	        {
		        return newMenu.Id;
	        }
            return Guid.Empty;
	        //List<MenuProduct> productsOfMenuToInsert = new List<MenuProduct>();
            //List<MenuStore> menuStoresToInsert = new List<MenuStore>();

            //string createByUserName = GetUsernameFromJwt();
            //DateTime currentDate = DateTime.Now;

            //newMenuRequest.Id = Guid.NewGuid();
            //newMenuRequest.Code = createNewMenuRequest.Code.Trim();
            //newMenuRequest.CreatedBy = createByUserName;
            //newMenuRequest.CreatedAt = currentDate;

            //if (createNewMenuRequest.ProductOfMenu.Count > 0)
            //{
            //    List<Product> currentProductsInSystem = (List<Product>)await _unitOfWork.GetRepository<Product>().GetListAsync();
            //    createNewMenuRequest.ProductOfMenu.ForEach(product =>
            //    {
            //        if (product.Id != Guid.Empty)
            //        {
            //            Product systemProductData = currentProductsInSystem.Find(x => x.Id == product.Id);
            //            if (systemProductData == null) throw new BadHttpRequestException(MessageConstant.Menu.ProductNotFoundMessage + product.Id);
            //            productsOfMenuToInsert.Add(new MenuProduct
            //            {
            //                Id = Guid.NewGuid(),
            //                ProductId = systemProductData.Id,
            //                MenuId = newMenuRequest.Id,
            //                Status = MenuProductStatus.Active.GetDescriptionFromEnum(),
            //                SellingPrice = product.SellingPrice,
            //                DiscountPrice = (double)(product.DiscountPrice == null ? 0 : product.DiscountPrice),
            //                HistoricalPrice = systemProductData.SellingPrice,
            //                CreatedBy = createByUserName,
            //                CreatedAt = currentDate
            //            });
            //        }
            //    });
            //}

            //if (createNewMenuRequest.StoreMenuAdditionInformation.Count > 0)
            //{
            //    List<Store> currentStoresInSystem = (List<Store>)await _unitOfWork.GetRepository<Store>().GetListAsync();
            //    createNewMenuRequest.StoreMenuAdditionInformation.ForEach(storeRequest =>
            //    {
            //        if (storeRequest.Id != Guid.Empty)
            //        {
            //            Store systemStoreData = currentStoresInSystem.Find(x => x.Id == storeRequest.Id);
            //            if (systemStoreData == null) throw new BadHttpRequestException(MessageConstant.Menu.StoreNotFoundMessage + storeRequest.Id);
            //            menuStoresToInsert.Add(new MenuStore
            //            {
            //                Id = Guid.NewGuid(),
            //                MenuId = newMenuRequest.Id,
            //                StoreId = systemStoreData.Id,
            //                Priority = storeRequest.Priority,
            //                DateFilter = storeRequest.DateFilter,
            //                Status = MenuProductStatus.Active.GetDescriptionFromEnum()
            //            });
            //        }
            //    });
            //}

            //await _unitOfWork.GetRepository<Menu>().InsertAsync(newMenuRequest);
            //await _unitOfWork.GetRepository<MenuProduct>().InsertRangeAsync(productsOfMenuToInsert);
            //await _unitOfWork.GetRepository<MenuStore>().InsertRangeAsync(menuStoresToInsert);
            //await _unitOfWork.CommitAsync();
        }
    }
}
