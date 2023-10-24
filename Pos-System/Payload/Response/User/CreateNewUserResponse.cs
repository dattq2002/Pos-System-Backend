using Pos_System.API.Enums;

namespace Pos_System.API.Payload.Response.User
{
    public class CreateNewUserResponse
    {
        public Guid Id;
        public string? PhoneNunmer;
        public string? FullName;
        public string? Gender;
        public string? Email;
        public UserStatus Status;
    }
}
