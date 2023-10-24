using AutoMapper;
using Pos_System.API.Payload.Request.User;
using Pos_System.API.Payload.Response.User;

namespace Pos_System.API.Mappers.User
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<CreateNewUserRequest, Pos_System.Domain.Models.User>();
            CreateMap<Pos_System.Domain.Models.User, CreateNewUserResponse>();
        }
    }
}
