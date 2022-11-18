using AutoMapper;
using Pos_System_Backend.Domain.Models;
using Pos_System_Backend.Repository.Interfaces;
using Pos_System_Backend.Services.Interfaces;

namespace Pos_System_Backend.Services.Implements;

public class MenuService : BaseService<MenuService>,IMenuService
{
	public MenuService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<MenuService> logger,IMapper mapper) : base(unitOfWork, logger, mapper)
	{
	}

	public void GetMenuOfStore()
	{
		throw new NotImplementedException();
	}

}