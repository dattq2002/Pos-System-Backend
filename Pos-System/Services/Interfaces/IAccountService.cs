using Pos_System.API.Payload.Request;
using Pos_System.API.Payload.Response;
using Pos_System.Domain.Models;

namespace Pos_System.API.Services.Interfaces
{
	public interface IAccountService
	{
		Task<LoginResponse> Login(LoginRequest loginRequest);
	}
}
