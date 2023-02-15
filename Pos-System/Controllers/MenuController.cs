using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Menus;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Validators;

namespace Pos_System.API.Controllers
{
    public class MenuController : BaseController<MenuController>
    {
        private readonly IMenuService _menuService;
        public MenuController(ILogger<MenuController> logger, IMenuService menuService) : base(logger)
        {
            _menuService = menuService;
        }

        [CustomAuthorize(RoleEnum.BrandAdmin)]
        [HttpPost(ApiEndPointConstant.Menu.MenuEndPoint)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewMenu(CreateNewMenuRequest createNewMenuRequest)
        {
            Guid newMenuIdResponse = await _menuService.CreateNewMenu(createNewMenuRequest);
            _logger.LogInformation($"Create menu successfully with menuCode: {createNewMenuRequest.Code}");
            return Ok(newMenuIdResponse);
        }
    }
}
