using System.ComponentModel.DataAnnotations;

namespace Pos_System.API.Payload.Response.User
{
    public class GetUserResponse
    {
        [Phone(ErrorMessage = "Phone sai format")]
        public string? PhoneNunmer { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        [EmailAddress(ErrorMessage = "Email sai format")]
        public string? Email { get; set; }

        public GetUserResponse(string? phoneNunmer, string? fullName, string? gender, string? email)
        {
            PhoneNunmer = phoneNunmer;
            FullName = fullName;
            Gender = gender;
            Email = email;
        }
    }
}
