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
using Pos_System.Domain.Paginate;
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
			Expression<Func<Account, bool>> searchFilter = p => p.Username.Equals(loginRequest.Username) && p.Password.Equals(PasswordUtil.HashPassword(loginRequest.Password));
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
				Id = Guid.NewGuid(),	
				AccountId = newBrandAccount.Id,
				BrandId = brand.Id
			};
			await _unitOfWork.GetRepository<Account>().InsertAsync(newBrandAccount);
			bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
			CreateNewBrandAccountResponse response = isSuccessful ? _mapper.Map<CreateNewBrandAccountResponse>(newBrandAccount) : null;
			return response;
		}

		public async Task<IPaginate<GetAccountResponse>> GetBrandAccounts(Guid brandId, string? searchUsername, RoleEnum role, int page, int size)
		{
			if (brandId == Guid.Empty) throw new BadHttpRequestException("Brand Id bị trống");
			IPaginate<GetAccountResponse> accountsInBrand = new Paginate<GetAccountResponse>();
			searchUsername = searchUsername?.Trim().ToLower();
			switch (role)
			{
				case RoleEnum.BrandAdmin:
				case RoleEnum.BrandManager:
					accountsInBrand = await _unitOfWork.GetRepository<Account>()
						.GetPagingListAsync(
							selector: x => new GetAccountResponse()
							{
								Id = x.Id, Username = x.Username, Name = x.Name,
								Role = EnumUtil.ParseEnum<RoleEnum>(x.Role.Name),
								Status = EnumUtil.ParseEnum<AccountStatus>(x.Status)
							},
							predicate: string.IsNullOrEmpty(searchUsername) ? x => x.BrandAccount != null && x.BrandAccount.BrandId.Equals(brandId) && x.Role.Name.Equals(role.GetDescriptionFromEnum())
								: x => x.BrandAccount != null && x.BrandAccount.BrandId.Equals(brandId) && x.Role.Name.Equals(role.GetDescriptionFromEnum()) && x.Username.ToLower().Contains(searchUsername),
							orderBy: x => x.OrderBy(x => x.Username),
							include: x => x.Include(x => x.BrandAccount).Include(x => x.Role),
							page: page,
							size: size);
					break;
				case RoleEnum.StoreManager:
				case RoleEnum.Staff:
					IEnumerable<Guid> storeIds = await _unitOfWork.GetRepository<Store>()
						.GetListAsync(selector: x => x.Id,predicate: x => x.BrandId.Equals(brandId));
					accountsInBrand = await _unitOfWork.GetRepository<Account>()
						.GetPagingListAsync(
							selector: x => new GetAccountResponse()
							{
								Id = x.Id,
								Username = x.Username,
								Name = x.Name,
								Role = EnumUtil.ParseEnum<RoleEnum>(x.Role.Name),
								Status = EnumUtil.ParseEnum<AccountStatus>(x.Status)
							},
							predicate: string.IsNullOrEmpty(searchUsername) ? x => x.StoreAccount != null && storeIds.OrEmpty().Contains(x.StoreAccount.StoreId) && x.Role.Name.Equals(role.GetDescriptionFromEnum()) 
								: x => x.StoreAccount != null && storeIds.OrEmpty().Contains(x.StoreAccount.StoreId) && x.Role.Name.Equals(role.GetDescriptionFromEnum()) && x.Username.ToLower().Contains(searchUsername),
							orderBy: x => x.OrderBy(x => x.Username),
							include: x => x.Include(x => x.StoreAccount),
							page: page,
							size: size);
					break;
			}
			return accountsInBrand;
		}
	}
}
