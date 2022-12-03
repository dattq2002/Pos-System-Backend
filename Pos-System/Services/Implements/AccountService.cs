using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Pos_System.API.Payload.Request;
using Pos_System.API.Payload.Response;
using Pos_System.API.Services;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements
{
	public class AccountService : BaseService<AccountService>, IAccountService
	{
		public AccountService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<AccountService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
		{
		}

		public async Task<LoginResponse> Login(LoginRequest loginRequest)
		{
			Expression<Func<Account, bool>> searchFilter = p => p.Username.Equals(loginRequest.Username) && p.Password.Equals(loginRequest.Password);
			Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: searchFilter, include: p => p.Include(x => x.Role));
			if (account == null) return null;
			var token = JwtUtil.GenerateJwtToken(account);
			var loginResponse = _mapper.Map<LoginResponse>(account);
			loginResponse.AccessToken = token;
			return loginResponse;
		}
	}
}
