using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Menus;
using Pos_System.API.Payload.Response.Menus;
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

        public async Task<GetMenuDetailResponse> GetMenuDetailInBrand(Guid brandId, Guid menuId)
        {
            if (brandId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.BrandNotFoundMessage);
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
    }
}
