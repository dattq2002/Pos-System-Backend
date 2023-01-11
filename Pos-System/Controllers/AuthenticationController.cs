using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request;
using Pos_System.API.Payload.Response;

namespace Pos_System.API.Controllers
{
	[ApiController]
	public class AuthenticationController : BaseController<AuthenticationController>
	{
		private readonly IAccountService _accountService;
        public AuthenticationController(ILogger<AuthenticationController> logger, IAccountService accountService) : base(logger)
		{
			_accountService = accountService;
		}


		[HttpPost(ApiEndPointConstant.Authentication.Login)]
		[ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
		[ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
		public async Task<IActionResult> Login(LoginRequest loginRequest)
		{
			var loginResponse = await _accountService.Login(loginRequest);
			if (loginResponse == null)
			{
				return Unauthorized(MessageConstant.LoginMessage.InvalidUsernameOrPassword);
			}
			if (loginResponse.Status == AccountStatus.Deactivate)
				return Unauthorized(MessageConstant.LoginMessage.DeactivatedAccount);
			return Ok(loginResponse);
		}
    }
}
