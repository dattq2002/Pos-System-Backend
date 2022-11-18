using Pos_System_Backend.Domain.Models;
using Pos_System_Backend.Models.Request;

namespace Pos_System_Backend.Services.Interfaces
{
	public interface IAccountService
	{
		Task<Account> Login(LoginRequest loginRequest);
	}
}
