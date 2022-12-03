using Pos_System.API.Enums;

namespace Pos_System.API.Models.Response;

public class LoginResponse
{
	public string AccessToken { get; set; }
	public Guid Id { get; set; }
	public string Username { get; set; }
	public string Name { get; set; }
    public RoleEnum Role { get; set; }
	public AccountStatus Status { get; set; }
}