using System.Security.Claims;
using AutoMapper;
using Microsoft.OpenApi.Extensions;
using Pos_System_Backend.Domain.Models;
using Pos_System_Backend.Enums;
using Pos_System_Backend.Repository.Interfaces;
using Pos_System_Backend.Services.Interfaces;

namespace Pos_System_Backend.Services.Implements;

public class MenuService : BaseService<MenuService>,IMenuService
{
	public MenuService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<MenuService> logger,IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
	{
	}
	public async Task<Store> GetMenuOfStore(Guid storeId, DateTime requestDateTime)
	{
		string status = StoreStatus.Active.GetDisplayName();
		Store store = await _unitOfWork.GetRepository<Store>()
			.SingleOrDefaultAsync(predicate: x => x.Id.Equals(storeId) && x.Status.Equals(status));
		var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType);
		return store;
	}
}