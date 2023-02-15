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
            Menu newMenuRequest = new Menu();
            List<MenuProduct> productsOfMenuToInsert = new List<MenuProduct>();
            List<MenuStore> menuStoresToInsert = new List<MenuStore>();

            string createByUserName = GetUsernameFromJwt();
            DateTime currentDate = DateTime.Now;

            newMenuRequest.Id = Guid.NewGuid();
            newMenuRequest.Code = createNewMenuRequest.Code.Trim();
            newMenuRequest.CreatedBy = createByUserName;
            newMenuRequest.CreatedAt = currentDate;

            if (createNewMenuRequest.ProductOfMenu.Count > 0)
            {
                List<Product> currentProductsInSystem = (List<Product>)await _unitOfWork.GetRepository<Product>().GetListAsync();
                createNewMenuRequest.ProductOfMenu.ForEach(product =>
                {
                    if (product.Id != Guid.Empty)
                    {
                        Product systemProductData = currentProductsInSystem.Find(x => x.Id == product.Id);
                        if (systemProductData == null) throw new BadHttpRequestException(MessageConstant.Menu.ProductNotFoundMessage + product.Id);
                        productsOfMenuToInsert.Add(new MenuProduct
                        {
                            Id = Guid.NewGuid(),
                            ProductId = systemProductData.Id,
                            MenuId = newMenuRequest.Id,
                            Status = MenuProductStatus.Active.GetDescriptionFromEnum(),
                            SellingPrice = product.SellingPrice,
                            DiscountPrice = (double)(product.DiscountPrice == null ? 0 : product.DiscountPrice),
                            HistoricalPrice = systemProductData.SellingPrice,
                            CreatedBy = createByUserName,
                            CreatedAt = currentDate
                        });
                    }
                });
            }

            if (createNewMenuRequest.StoreMenuAdditionInformation.Count > 0)
            {
                List<Store> currentStoresInSystem = (List<Store>)await _unitOfWork.GetRepository<Store>().GetListAsync();
                createNewMenuRequest.StoreMenuAdditionInformation.ForEach(storeRequest =>
                {
                    if (storeRequest.Id != Guid.Empty)
                    {
                        Store systemStoreData = currentStoresInSystem.Find(x => x.Id == storeRequest.Id);
                        if (systemStoreData == null) throw new BadHttpRequestException(MessageConstant.Menu.StoreNotFoundMessage + storeRequest.Id);
                        menuStoresToInsert.Add(new MenuStore
                        {
                            Id = Guid.NewGuid(),
                            MenuId = newMenuRequest.Id,
                            StoreId = systemStoreData.Id,
                            Priority = storeRequest.Priority,
                            DateFilter = storeRequest.DateFilter,
                            TimeFilter = storeRequest.TimeFilter,
                            Status = MenuProductStatus.Active.GetDescriptionFromEnum()
                        });
                    }
                });
            }

            await _unitOfWork.GetRepository<Menu>().InsertAsync(newMenuRequest);
            await _unitOfWork.GetRepository<MenuProduct>().InsertRangeAsync(productsOfMenuToInsert);
            await _unitOfWork.GetRepository<MenuStore>().InsertRangeAsync(menuStoresToInsert);
            await _unitOfWork.CommitAsync();

            return newMenuRequest.Id;
        }
    }
}
