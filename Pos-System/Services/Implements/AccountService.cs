using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request;
using Pos_System.API.Payload.Request.Brands;
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

		public async Task<CreateNewBrandAccountResponse> CreateNewBrandAccount(CreateNewBrandAccountRequest createNewBrandAccountRequest)
		{
			Brand brand = await _unitOfWork.GetRepository<Brand>()
				.SingleOrDefaultAsync(predicate: x => x.Id.Equals(createNewBrandAccountRequest.BrandId));
			if (brand == null) throw new BadHttpRequestException(MessageConstant.Brand.BrandNotFoundMessage);
			if (createNewBrandAccountRequest.Role != RoleEnum.BrandAdmin &&
			    createNewBrandAccountRequest.Role != RoleEnum.BrandManager) throw new BadHttpRequestException(MessageConstant.Account.CreateAccountWithWrongRoleMessage);
			_logger.LogInformation($"Create new account for brand {brand.Name} with account {createNewBrandAccountRequest.Username}, role: {createNewBrandAccountRequest.Role.GetDescriptionFromEnum()} ");
			createNewBrandAccountRequest.Password = PasswordUtil.HashPassword(createNewBrandAccountRequest.Password);
			Account newBrandAccount = _mapper.Map<Account>(createNewBrandAccountRequest);
			newBrandAccount.Id = Guid.NewGuid();
			newBrandAccount.RoleId = await _unitOfWork.GetRepository<Role>().SingleOrDefaultAsync(selector: x => x.Id,predicate: x => x.Name.Equals(createNewBrandAccountRequest.Role.GetDescriptionFromEnum()));
			newBrandAccount.BrandAccount = new BrandAccount()
			{
				AccountId = newBrandAccount.Id,
				BrandId = brand.Id
			};
			await _unitOfWork.GetRepository<Account>().InsertAsync(newBrandAccount);
			bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
			CreateNewBrandAccountResponse response = isSuccessful ? _mapper.Map<CreateNewBrandAccountResponse>(newBrandAccount) : null;
			return response;
		}
	}
}
