using System.ComponentModel.DataAnnotations;

namespace Pos_System_Backend.Models.Request;

public class LoginRequest
{
	[Required(ErrorMessage = "Username is required")]
	[MaxLength(50)]
	public string Username { get; set; }
	[Required(ErrorMessage = "Password is required")]
	public string Password { get; set; }
}