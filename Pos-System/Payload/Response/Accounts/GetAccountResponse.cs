using Pos_System.API.Enums;

namespace Pos_System.API.Payload.Response;

public class GetAccountResponse
{
	public Guid Id { get; set; }
	public string Username { get; set; }
	public string Name { get; set; }
	public RoleEnum Role { get; set; }
	public AccountStatus Status { get; set; }
}