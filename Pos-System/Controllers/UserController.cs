using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Payload.Request.User;
using Pos_System.API.Payload.Response.User;
using Pos_System.API.Services.Implements;
using Pos_System.API.Services.Interfaces;

namespace Pos_System.API.Controllers
{
    [ApiController]
    public class UserController : BaseController<UserController>
    {
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService) : base(logger)
        {
            _userService = userService;
        }


        [HttpPost(ApiEndPointConstant.User.UsersEndpoint)]
        [ProducesResponseType(typeof(CreateNewUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNewUser([FromBody]CreateNewUserRequest newUserRequest, [FromQuery]string? brandCode)
        {
            CreateNewUserResponse response = await _userService.CreateNewUser(newUserRequest, brandCode);
            if (response == null)
            {
                _logger.LogError($"Create new user failed with {newUserRequest.FullName}");
                return Problem($"{MessageConstant.User.CreateNewUserFailedMessage}: {newUserRequest.FullName}");
            }

            _logger.LogInformation($"Create new user successful with {newUserRequest.FullName}");
            return CreatedAtAction(nameof(CreateNewUser), response);
        }

        [HttpPatch(ApiEndPointConstant.User.UserEndpoint)]
        public async Task<IActionResult> UpdateUserInformation(Guid id, [FromBody]UpdateUserRequest updateUserRequest)
        {
            bool isSuccessful = await _userService.UpdateUserInformation(id, updateUserRequest);
            if (isSuccessful)
            {
                _logger.LogInformation($"Update user {id} information successfully");
                return Ok(MessageConstant.User.UpdateUserSuccessfulMessage);
            }

            _logger.LogInformation($"Update Brand {id} information failed");
            return Ok(MessageConstant.User.UpdateUserFailedMessage);
        }

        [HttpGet(ApiEndPointConstant.User.UserEndpoint)]
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var userResponse = _userService.GetUserById(userId);
            return Ok(userResponse);
        }
    }
}
