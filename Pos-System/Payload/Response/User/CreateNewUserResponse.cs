using Pos_System.API.Enums;

namespace Pos_System.API.Payload.Response.User
{
    public class CreateNewUserResponse
    {
        public Guid Id { get; set; }
        public string? PhoneNunmer { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public UserStatus Status { get; set; }
    }
}
