using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Menus;
using Pos_System.API.Payload.Response.Menus;
using Pos_System.API.Payload.Response.Products;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Domain.Paginate;
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
            Guid brandId = Guid.Parse(GetBrandIdFromJwt());
            if (brandId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.EmptyBrandIdMessage);
            Brand brand = await _unitOfWork.GetRepository<Brand>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(brandId));
            if (brand == null) throw new BadHttpRequestException(MessageConstant.Brand.BrandNotFoundMessage);
            _logger.LogInformation($"Create new Menu with menu code: {createNewMenuRequest.Code}");
            Menu newMenu = new Menu()
            {
                Id = Guid.NewGuid(),
                Code = createNewMenuRequest.Code.Trim(),
                DateFilter = createNewMenuRequest.DateFilter,
                Priority = createNewMenuRequest.Priority,
                StartTime = createNewMenuRequest.StartTime,
                EndTime = createNewMenuRequest.EndTime,
                BrandId = brand.Id,
                CreatedBy = GetUsernameFromJwt(),
                CreatedAt = DateTime.UtcNow,
                Status = EnumUtil.GetDescriptionFromEnum(MenuStatus.Deactivate)
            };
            if (createNewMenuRequest.IsBaseMenu)
            {
                newMenu.Priority = 0; //Default priority of base menu is 0
                newMenu.MenuStores = new List<MenuStore>();
                IEnumerable<Guid> storesInBrand = await _unitOfWork.GetRepository<Store>()
                    .GetListAsync(
                        selector: x => x.Id,
                        predicate: x => x.BrandId.Equals(brand.Id));
                foreach (var storeGuid in storesInBrand)
                {
                    newMenu.MenuStores.Add(new MenuStore()
                    {
                        Id = Guid.NewGuid(),
                        MenuId = newMenu.Id,
                        StoreId = storeGuid
                    });
                }
            } //Create a base menu for brand that will apply for all stores
            await _unitOfWork.GetRepository<Menu>().InsertAsync(newMenu);
            bool isSuccessfully = await _unitOfWork.CommitAsync() > 0;
            if (isSuccessfully) return newMenu.Id;
            return Guid.Empty;
        }

        public async Task<HasBaseMenuResponse> CheckHasBaseMenuInBrand(Guid brandId)
        {
            if (brandId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.EmptyBrandIdMessage);
            Brand brand = await _unitOfWork.GetRepository<Brand>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(brandId));
            if (brand == null) throw new BadHttpRequestException(MessageConstant.Brand.BrandNotFoundMessage);
            IEnumerable<Menu> menus = await _unitOfWork.GetRepository<Menu>()
                .GetListAsync(predicate: x => x.BrandId.Equals(brand.Id));
            if (menus == null)
            {
                return new HasBaseMenuResponse()
                {
                    HasBaseMenu = false
                };
            }// brand đó chưa có menu nào

            if (menus.Any(x => x.Priority == 0))
            {
                return new HasBaseMenuResponse()
                {
                    HasBaseMenu = true
                };
            }
            else
            {
                return new HasBaseMenuResponse()
                {
                    HasBaseMenu = false
                };
            }
        }

        public async Task<IPaginate<GetMenuDetailResponse>> GetMenus(Guid brandId, string? code, int page = 1, int size = 10)
        {
            if (brandId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.BrandNotFoundMessage);
            Brand brand = await _unitOfWork.GetRepository<Brand>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(brandId));
            if (brand == null) throw new BadHttpRequestException(MessageConstant.Brand.BrandNotFoundMessage);
            code = code?.Trim();
            IPaginate<GetMenuDetailResponse> menusInBrand = await _unitOfWork.GetRepository<Menu>().GetPagingListAsync(
                selector: x => new GetMenuDetailResponse(x.Id, x.Code, x.Priority, x.DateFilter, x.StartTime, x.EndTime, x.Status, x.CreatedBy, x.CreatedAt, x.UpdatedBy, x.UpdatedAt, x.MenuProducts.ToList(), x.MenuStores.ToList()),
                predicate: string.IsNullOrEmpty(code) ? x => x.BrandId.Equals(brandId) : x => x.Code.ToLower().Equals(code) && x.BrandId.Equals(brandId),
                include: x => x.Include(x => x.MenuStores).ThenInclude(x => x.Store).Include(x => x.MenuProducts).ThenInclude(x => x.Product).ThenInclude(x => x.Category),
                page: page,
                size: size
            );
            return menusInBrand;
        }

        public async Task<GetMenuDetailResponse> GetMenuDetailInBrand(Guid menuId)
        {
            Guid brandId = Guid.Parse(GetBrandIdFromJwt());
            Brand brand = await _unitOfWork.GetRepository<Brand>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(brandId));
            if (brand == null) throw new BadHttpRequestException(MessageConstant.Brand.BrandNotFoundMessage);

            GetMenuDetailResponse menuDetailResponse = await _unitOfWork.GetRepository<Menu>().SingleOrDefaultAsync(
                selector: menu => new GetMenuDetailResponse(menu.Id, menu.Code, menu.Priority, menu.DateFilter,
                    menu.StartTime, menu.EndTime,
                    menu.Status, menu.CreatedBy, menu.CreatedAt, menu.UpdatedBy, menu.UpdatedAt,
                    menu.MenuProducts.ToList(), menu.MenuStores.ToList()),
                predicate: menu => menu.Id.Equals(menuId) && menu.BrandId.Equals(brandId),
                include: menu => menu.Include(menu => menu.MenuProducts).ThenInclude(menu => menu.Product).ThenInclude(menu => menu.Category).Include(menu => menu.MenuStores).ThenInclude(menu => menu.Store)
            );

            if (menuDetailResponse == null)
            {
                throw new BadHttpRequestException(MessageConstant.Menu.BrandIdWithMenuIdIsNotExistedMessage);
            }
            return menuDetailResponse;
        }

        public async Task<Guid> UpdateMenuProducts(Guid menuId, UpdateMenuProductsRequest updateMenuProductsRequest)
        {
            if (menuId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Menu.EmptyMenuIdMessage);
            Menu menu = await _unitOfWork.GetRepository<Menu>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(menuId));
            if (menu == null) throw new BadHttpRequestException(MessageConstant.Menu.MenuNotFoundMessage);

            string currentUserName = GetUsernameFromJwt();
            Guid userBrandId = Guid.Parse(GetBrandIdFromJwt());
            DateTime currentTime = DateTime.Now;

            List<MenuProduct> productsInMenu = (List<MenuProduct>)await _unitOfWork.GetRepository<MenuProduct>().GetListAsync(predicate: x => x.MenuId.Equals(menuId));
            List<Product> currentProductsInSystem = (List<Product>)await _unitOfWork
                    .GetRepository<Product>()
                    .GetListAsync(predicate: x => x.BrandId.Equals(userBrandId));

            List<Guid> newProductIds = updateMenuProductsRequest.Products.Select(x => x.ProductId).ToList();
            List<Guid> oldProductIds = productsInMenu.Select(x => x.ProductId).ToList();
            (List<Guid> idsToRemove, List<Guid> idsToAdd, List<Guid> idsToKeep) splittedProductIds = CustomListUtil.splitIdsToAddAndRemove(oldProductIds, newProductIds);

            if (splittedProductIds.idsToAdd.Count > 0)
            {
                List<ProductToUpdate> productsToInsert = updateMenuProductsRequest.Products
                    .Where(x => splittedProductIds.idsToAdd.Contains(x.ProductId)).ToList();

                List<MenuProduct> prepareDataToInsert = new List<MenuProduct>();
                productsToInsert.ForEach(x =>
                {
                    Product referenceProductData = currentProductsInSystem.Find(y => y.Id.Equals(x.ProductId));
                    if (referenceProductData == null) throw new BadHttpRequestException(MessageConstant.Menu.ProductNotInBrandMessage + x.ProductId);
                    prepareDataToInsert.Add(new MenuProduct
                    {
                        Id = Guid.NewGuid(),
                        Status = ProductStatus.Active.GetDescriptionFromEnum(),
                        SellingPrice = (double)(x.SellingPrice == null ? referenceProductData.SellingPrice : x.SellingPrice),
                        DiscountPrice = (double)(x.DiscountPrice == null ? referenceProductData.DiscountPrice : x.DiscountPrice),
                        HistoricalPrice = referenceProductData.SellingPrice,
                        MenuId = menuId,
                        ProductId = x.ProductId,
                        CreatedBy = currentUserName,
                        CreatedAt = currentTime,
                    });
                });

                await _unitOfWork.GetRepository<MenuProduct>().InsertRangeAsync(prepareDataToInsert);
            }

            if (splittedProductIds.idsToKeep.Count > 0)
            {
                List<ProductToUpdate> productDataFromRequest = updateMenuProductsRequest.Products
                    .Where(x => splittedProductIds.idsToKeep.Contains(x.ProductId)).ToList();
                List<MenuProduct> productsToUpdate = productsInMenu
                    .Where(x => splittedProductIds.idsToKeep.Contains(x.ProductId)).ToList();

                productsToUpdate.ForEach(x =>
                {
                    ProductToUpdate requestProductData = productDataFromRequest.Find(y => y.ProductId.Equals(x.ProductId));
                    if (requestProductData == null) return;
                    x.SellingPrice = (double)(requestProductData.SellingPrice == null ? x.SellingPrice : requestProductData.SellingPrice);
                    x.DiscountPrice = (double)(requestProductData.DiscountPrice == null ? x.DiscountPrice : requestProductData.DiscountPrice);
                    x.UpdatedBy = currentUserName;
                    x.UpdatedAt = currentTime;
                });

                _unitOfWork.GetRepository<MenuProduct>().UpdateRange(productsToUpdate);
            }

            if (splittedProductIds.idsToRemove.Count > 0)
            {
                List<MenuProduct> prepareDataToRemove = (List<MenuProduct>)await _unitOfWork.GetRepository<MenuProduct>().GetListAsync(
                    predicate: x => splittedProductIds.idsToRemove.Contains(x.ProductId)
                    && x.MenuId.Equals(menuId));

                _unitOfWork.GetRepository<MenuProduct>().DeleteRangeAsync(prepareDataToRemove);
            }

            await _unitOfWork.CommitAsync();
            return menuId;
        }

        public async Task<IPaginate<GetProductInMenuResponse>> GetProductsInMenu(Guid menuId, string? productName, string? productCode, int page, int size)
        {
            Guid brandId = Guid.Parse(GetBrandIdFromJwt());
            Brand brand = await _unitOfWork.GetRepository<Brand>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(brandId));
            if (brand == null) throw new BadHttpRequestException(MessageConstant.Brand.BrandNotFoundMessage);
            IPaginate<GetProductInMenuResponse> productsInMenu;

            if (!string.IsNullOrEmpty(productName) && !string.IsNullOrEmpty(productCode))
            {
                productsInMenu = await _unitOfWork.GetRepository<MenuProduct>()
                    .GetPagingListAsync(
                        selector: product => new GetProductInMenuResponse(product.ProductId, product.Product.Name, product.Product.Code, product.Product.PicUrl, product.SellingPrice,
                            product.HistoricalPrice, product.DiscountPrice),
                        predicate: product => product.MenuId.Equals(menuId) && product.Product.BrandId.Equals(brandId) && product.Product.Name.Contains(productName) && product.Product.Code.Contains(productCode),
                        include: product => product.Include(product => product.Product),
                        page: page,
                        size: size
                    );
            }
            else if (!string.IsNullOrEmpty(productName))
            {
                productsInMenu = await _unitOfWork.GetRepository<MenuProduct>()
                    .GetPagingListAsync(
                        selector: product => new GetProductInMenuResponse(product.ProductId, product.Product.Name, product.Product.Code, product.Product.PicUrl, product.SellingPrice,
                            product.HistoricalPrice, product.DiscountPrice),
                        predicate: product => product.MenuId.Equals(menuId) && product.Product.BrandId.Equals(brandId) && product.Product.Name.Contains(productName),
                        include: product => product.Include(product => product.Product),
                        page: page,
                        size: size
                    );
            }
            else if (!string.IsNullOrEmpty(productCode))
            {
                productsInMenu = await _unitOfWork.GetRepository<MenuProduct>()
                    .GetPagingListAsync(
                        selector: product => new GetProductInMenuResponse(product.ProductId, product.Product.Name, product.Product.Code, product.Product.PicUrl, product.SellingPrice,
                            product.HistoricalPrice, product.DiscountPrice),
                        predicate: product => product.MenuId.Equals(menuId) && product.Product.BrandId.Equals(brandId) && product.Product.Code.Contains(productCode),
                        include: product => product.Include(product => product.Product),
                        page: page,
                        size: size
                    );
            }
            else
            {
                productsInMenu = await _unitOfWork.GetRepository<MenuProduct>()
                    .GetPagingListAsync(
                        selector: product => new GetProductInMenuResponse(product.ProductId, product.Product.Name, product.Product.Code, product.Product.PicUrl, product.SellingPrice,
                            product.HistoricalPrice, product.DiscountPrice),
                        predicate: product => product.MenuId.Equals(menuId) && product.Product.BrandId.Equals(brandId),
                        include: product => product.Include(product => product.Product),
                        page: page,
                        size: size
                    );
            }
            return productsInMenu;
        }

        public async Task<bool> UpdateMenuInformation(Guid menuId, UpdateMenuInformationRequest updateMenuInformationRequest)
        {
            Guid branId = Guid.Parse(GetBrandIdFromJwt());

            if (updateMenuInformationRequest.Priority == 0)
            {
                Menu brandBaseMenu = await _unitOfWork.GetRepository<Menu>().SingleOrDefaultAsync(
                    predicate: menu => menu.BrandId.Equals(branId) && menu.Priority == 0
                );
                if (brandBaseMenu != null)
                {
                    _logger.LogInformation($"Failed to update menu {menuId} because brand has base menu already");
                    throw new BadHttpRequestException(MessageConstant.Menu.BaseMenuIsExistedInBrandMessage);
                }
            }

            Menu currentMenu = await _unitOfWork.GetRepository<Menu>().SingleOrDefaultAsync(
                predicate: menu => menu.BrandId.Equals(branId) && menu.Id.Equals(menuId)
            );

            if (currentMenu == null)
            {
                _logger.LogInformation($"Failed to update menu {menuId} because of wrong menuId or brandId from JWT");
                throw new BadHttpRequestException(MessageConstant.Menu.BrandIdWithMenuIdIsNotExistedMessage);
            }

            if (updateMenuInformationRequest.StartTime.HasValue && (updateMenuInformationRequest.StartTime > currentMenu.EndTime))
            {
                throw new BadHttpRequestException(MessageConstant.Menu.EndTimeLowerThanStartTimeMessage);
            }

            if (updateMenuInformationRequest.EndTime.HasValue && (updateMenuInformationRequest.EndTime < currentMenu.StartTime))
            {
                throw new BadHttpRequestException(MessageConstant.Menu.EndTimeLowerThanStartTimeMessage);
            }

            if (updateMenuInformationRequest.StartTime.HasValue && updateMenuInformationRequest.EndTime.HasValue
                && updateMenuInformationRequest.StartTime > updateMenuInformationRequest.EndTime)
            {
                _logger.LogInformation($"Failed to update menu {menuId} because endtime is lower than starttime");
                throw new BadHttpRequestException(MessageConstant.Menu.EndTimeLowerThanStartTimeMessage);
            }

            _logger.LogInformation($"Start update menu information of menu's {menuId} id");
            currentMenu.Priority = updateMenuInformationRequest.Priority ?? currentMenu.Priority;
            currentMenu.DateFilter = updateMenuInformationRequest.DateFilter ?? currentMenu.DateFilter;
            currentMenu.StartTime = updateMenuInformationRequest.StartTime ?? currentMenu.StartTime;
            currentMenu.EndTime = updateMenuInformationRequest.EndTime ?? currentMenu.EndTime;
            _unitOfWork.GetRepository<Menu>().UpdateAsync(currentMenu);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
