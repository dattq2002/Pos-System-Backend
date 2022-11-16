using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Pos_System_Backend.Domain.Models;
using Pos_System_Backend.Models.Request;
using Pos_System_Backend.Repository.Interfaces;
using Pos_System_Backend.Services.Interfaces;

namespace Pos_System_Backend.Services.Implements
{
	public class AccountService : BaseService<AccountService>, IAccountService
	{
		public AccountService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<AccountService> logger) : base(unitOfWork, logger)
		{
		}

		public async Task<Account> Login(LoginRequest loginRequest)
		{
			Expression<Func<Account,bool>> searchFilter = p => p.Username.Equals(loginRequest.Username) && p.Password.Equals(loginRequest.Password);
			Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: searchFilter, include: p=> p.Include(x => x.Role));
			return account;
		}
	}
}
