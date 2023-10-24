using AutoMapper;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Brands;
using Pos_System.API.Payload.Request.User;
using Pos_System.API.Payload.Response.User;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements
{
    public class UserService : BaseService<UserService>, IUserService
    {
        public UserService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<UserService> logger,
            IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewUserResponse> CreateNewUser(CreateNewUserRequest newUserRequest, string? brandCode)
        {
            if (brandCode == null) throw new BadHttpRequestException(MessageConstant.Brand.EmptyBrandCodeMessage);
            Brand brand = await _unitOfWork.GetRepository<Brand>()
                .SingleOrDefaultAsync(predicate: x => x.BrandCode.Equals(brandCode));
            if (brand == null) throw new BadHttpRequestException(MessageConstant.Brand.BrandNotFoundMessage);

            _logger.LogInformation($"Create new brand with {newUserRequest.FullName}");
            User newUser = _mapper.Map<User>(newUserRequest);
            newUser.Status = UserStatus.Active.GetDescriptionFromEnum();
            newUser.Id = Guid.NewGuid();
            newUser.BrandId = brand.Id;

            newUser.FireBaseUid = "";
            newUser.Fcmtoken = "";

            newUser.CreatedAt = DateTime.UtcNow;
            newUser.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.GetRepository<User>().InsertAsync(newUser);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            CreateNewUserResponse createNewUserResponse = null;
            if (isSuccessful)
            {
                createNewUserResponse = _mapper.Map<CreateNewUserResponse>(newUser);
            }

            return createNewUserResponse;
        }

        public async Task<bool> UpdateUserInformation(Guid userId, UpdateUserRequest updatedUserRequest)
        {
            if (userId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.User.EmptyUserId);
            User updatedUser = await _unitOfWork.GetRepository<User>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(userId));

            if (updatedUser == null) throw new BadHttpRequestException(MessageConstant.User.UserNotFound);

            _logger.LogInformation($"Start update user {userId}");
            updatedUserRequest.TrimString();
            updatedUser.FullName = string.IsNullOrEmpty(updatedUserRequest.FullName) ? updatedUser.FullName : updatedUserRequest.FullName;
            updatedUser.Email = string.IsNullOrEmpty(updatedUserRequest.Email) ? updatedUser.Email : updatedUserRequest.Email;
            updatedUser.Gender = string.IsNullOrEmpty(updatedUserRequest.Gender) ? updatedUser.Gender : updatedUserRequest.Gender;
            updatedUser.PhoneNumber = string.IsNullOrEmpty(updatedUserRequest.PhoneNunmer) ? updatedUser.PhoneNumber : updatedUserRequest.PhoneNunmer;
            _unitOfWork.GetRepository<User>().UpdateAsync(updatedUser);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<GetUserResponse> GetUserById(Guid userId)
        {
            if(userId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.User.EmptyUserId);

            GetUserResponse userResponse = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
                selector: x => new GetUserResponse(x.PhoneNumber, x.FullName, x.Gender, x.Email),
                predicate: x => x.Id.Equals(userId));

            return userResponse;
        }
    }
}
