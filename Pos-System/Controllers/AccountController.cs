using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
		public async Task<IActionResult> Login(LoginRequest loginRequest)
		{
			var loginReponse = await _accountService.Login(loginRequest);
			if (loginReponse == null)
			{
				return BadRequest(LoginMessage.InvalidUsernameOrPassword);
			}
			if (loginReponse.Status == AccountStatus.Deactivated)
				return BadRequest(LoginMessage.DeactivatedAccount);
			return Ok(loginReponse);
		}

		[CustomAuthorize(RoleEnum.Admin)]
		[HttpGet(ControllerName)]
		public IActionResult GetOk()
		{

			return Ok();
		}
	}
}
