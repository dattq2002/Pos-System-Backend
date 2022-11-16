using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System_Backend.Constants;
using Pos_System_Backend.Domain.Models;
using Pos_System_Backend.Models.Request;
using Pos_System_Backend.Repository.Interfaces;
using Pos_System_Backend.Services.Interfaces;
using Pos_System_Backend.Utils;

namespace Pos_System_Backend.Controllers
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
		public async Task<IActionResult> GetAccount(LoginRequest loginRequest)
		{
			var account = await _accountService.Login(loginRequest);
			var token = JwtUtil.GenerateJwtToken(account);
			return Ok(token);
		}

		[Authorize(Roles = RoleConstant.Employee)]
		[HttpGet(ControllerName)]
		public IActionResult GetOk()
		{
			
			return Ok();
		}
	}
}
