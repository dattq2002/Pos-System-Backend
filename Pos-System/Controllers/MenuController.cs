using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Services.Interfaces;

namespace Pos_System.API.Controllers
{
	[ApiController]
	public class MenuController : BaseController<MenuController>
	{
		private const string ControllerName = "menu";
		private readonly IMenuService _menuService;
		public MenuController(ILogger<MenuController> logger, IMenuService menuService) : base(logger)
		{
			_menuService = menuService;
		}

		[Authorize(Roles = $"{RoleConstant.Employee},{RoleConstant.BrandManager},{RoleConstant.StoreManager}")]
		[HttpGet(ControllerName)]
		public async Task<IActionResult> GetMenu([FromQuery] Guid storeId, [FromQuery] DateTime? inputDateTime)
		{
			DateTime requestDateTime = inputDateTime ?? DateTime.Now;
			var menuResponse = await _menuService.GetMenuOfStore(storeId, requestDateTime);
			return Ok(menuResponse);
		}
	}
}
