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
    }
}
