using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.Domain.Models;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Constants;
using Pos_System.API.Models.Request;
using Pos_System.API.Utils;

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
			var account = await _accountService.Login(loginRequest);
			if (account == null)
			{
				return BadRequest("Username or password is not correct");
			}
			var token = JwtUtil.GenerateJwtToken(account);
			return Ok(token);
		}

		[Authorize]
		[HttpGet(ControllerName)]
		public IActionResult GetOk()
		{

			return Ok();
		}
	}
}
