using System.ComponentModel.DataAnnotations;

namespace Pos_System.API.Payload.Request.User
{
    public class UpdateUserRequest
    {
        [Phone(ErrorMessage = "Phone sai format")]
        public string? PhoneNunmer;
        public string? FullName;
        public string? Gender;
        [EmailAddress(ErrorMessage = "Email sai format")]
        public string? Email;

        public void TrimString()
        {
            FullName = FullName?.Trim();
            Gender = Gender?.Trim();
            Email = Email?.Trim();
            PhoneNunmer = PhoneNunmer?.Trim();
    }
    }
}
