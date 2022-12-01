using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Services.Interfaces;
using Pos_System.Domain.Models;

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

		[Authorize]
		[HttpGet(ControllerName)]
		public async Task<IActionResult> GetMenu()
		{
			return Ok();
		}
	}
}
