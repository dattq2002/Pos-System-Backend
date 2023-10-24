using System.ComponentModel.DataAnnotations;

namespace Pos_System.API.Payload.Response.User
{
    public class GetUserResponse
    {
        [Phone(ErrorMessage = "Phone sai format")]
        public string? PhoneNunmer;
        public string? FullName;
        public string? Gender;
        [EmailAddress(ErrorMessage = "Email sai format")]
        public string? Email;

        public GetUserResponse(string? phoneNunmer, string? fullName, string? gender, string? email)
        {
            PhoneNunmer = phoneNunmer;
            FullName = fullName;
            Gender = gender;
            Email = email;
        }
    }
}
