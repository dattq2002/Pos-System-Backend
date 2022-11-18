using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Pos_System.API.Enums;
using Pos_System.API.Helpers;
using Pos_System.API.Models.Response.Menu;
using Pos_System.API.Models.Response.Product;
using Pos_System.API.Services;
using Pos_System.API.Services.Interfaces;
using Pos_System.Domain.Models;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements;

public class MenuService : BaseService<MenuService>, IMenuService
{
	public MenuService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<MenuService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
	{
	}
	public async Task<MenuResponse> GetMenuOfStore(Guid storeId, DateTime requestDateTime)
	{
		Store store = await _unitOfWork.GetRepository<Store>()
			.SingleOrDefaultAsync(predicate: x => x.Id.Equals(storeId) && x.Status.Equals(StoreStatus.Active.ToString()));
		var username = GetUsernameFromJwt();
		Account accountStore = await _unitOfWork.GetRepository<Account>()
			.SingleOrDefaultAsync(predicate: s => s.Username.Equals(username));
		if (accountStore == null) return null;
		bool isAccountInStore = await CheckIsUserInStore(accountStore, store);
		if (isAccountInStore)
		{
			_logger.LogInformation($"Account {accountStore.Username} gets Menu for {store.Code}");
			//Get Menu 
			//Add date filter and time filter later in here
			DateFilter  dateFilter = DateTimeHelper.GetDateFromDateTime(requestDateTime);
			MenuStore menuInStore = await _unitOfWork.GetRepository<MenuStore>()
				.SingleOrDefaultAsync(predicate: p => p.StoreId.Equals(store.Id) && ((p.DateFilter & (int)dateFilter) > 0));

			Menu menu = await _unitOfWork.GetRepository<Menu>()
				.SingleOrDefaultAsync(predicate: x => x.Id.Equals(menuInStore.MenuId));
			//Get product from Menu
			IEnumerable<MenuProduct> menuProduct = await _unitOfWork.GetRepository<MenuProduct>()
				.GetListAsync(predicate: x => x.MenuId.Equals(menu.Id), include: q => q.Include(q => q.Product).ThenInclude(x => x.CategoryCodeNavigation));
			//Get category extra
			List<string> categoriesCode = menuProduct.Where(x => x.Product.Type.Equals(ProductType.HAS_EXTRA.ToString())).Select(x => x.Product.CategoryCode).ToList();

			//Filter Product
			menuProduct = menuProduct.Where(x => x.Product.Status.Equals(ProductStatus.Active.ToString()) && (x.Product.Type.Equals(ProductType.SINGLE.ToString()) || x.Product.Type.Equals(ProductType.HAS_EXTRA.ToString())));
			//Get extra Category 
			//Mapping to Menu Response
			MenuResponse menuResponse = _mapper.Map<MenuResponse>(menu);
			Array.ForEach(menuProduct.ToArray(), product =>
			{
				ProductInMenuResponse productInMenuResponse = _mapper.Map<ProductInMenuResponse>(product);
				menuResponse.Products.Add(productInMenuResponse);
			});
			return menuResponse;
		}

		return new MenuResponse();
	}
}