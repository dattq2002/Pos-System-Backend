using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Pos_System.Domain.Models;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Models.Request;
using Pos_System.API.Models.Response;
using Pos_System.API.Utils;
using Pos_System.API.Validators;

namespace Pos_System.API.Controllers
{
	[ApiController]
	public class AccountController : BaseController<AccountController>
	{
		private readonly IAccountService _accountService;
		private const string ControllerName = "accounts";
		public AccountController(ILogger<AccountController> logger, IAccountService accountService) : base(logger)
		{
			_accountService = accountService;
		}


		[HttpPost(ControllerName + "/login")]
		[ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
		public async Task<IActionResult> Login(LoginRequest loginRequest)
		{
			var loginResponse = await _accountService.Login(loginRequest);
			if (loginResponse == null)
			{
				return Unauthorized(LoginMessage.InvalidUsernameOrPassword);
			}
			if (loginResponse.Status == AccountStatus.Deactivated)
				return Unauthorized(LoginMessage.DeactivatedAccount);
			return Ok(loginResponse);
		}

		[CustomAuthorize(RoleEnum.Admin, RoleEnum.BrandAdmin, RoleEnum.BrandManager, RoleEnum.Staff, RoleEnum.StoreManager)]
		[HttpGet(ControllerName)]
		public IActionResult GetOk()
		{

			return Ok();
		}
	}
}
