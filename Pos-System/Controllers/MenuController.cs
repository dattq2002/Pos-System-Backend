using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System_Backend.Constants;

namespace Pos_System_Backend.Controllers
{
	[ApiController]
	public class MenuController : BaseController<MenuController>
	{
		private const string ControllerName = "menu";
		public MenuController(ILogger<MenuController> logger) : base(logger)
		{
		}

		[Authorize(Roles = $"{RoleConstant.Employee},{RoleConstant.BrandManager},{RoleConstant.StoreManager}")]
		[HttpGet(ControllerName)]
		public async Task<IActionResult> GetMenu()
		{
			return Ok();
		}
	}
}
