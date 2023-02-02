using Pos_System.API.Enums;
using Pos_System.API.Utils;

namespace Pos_System.API.Payload.Response;

public class LoginResponse
{
	public string AccessToken { get; set; }
	public Guid Id { get; set; }
	public string Username { get; set; }
	public string Name { get; set; }
    public RoleEnum Role { get; set; }
	public AccountStatus Status { get; set; }

	public LoginResponse(Guid id, string username, string name, string role, string status)
	{
		Id = id;
		Username = username;
		Name = name;
		Role = EnumUtil.ParseEnum<RoleEnum>(role);
		Status = EnumUtil.ParseEnum<AccountStatus>(status);
	}
}

public class BrandAccountLoginResponse : LoginResponse
{
	public Guid BrandId { get; set; }


	public BrandAccountLoginResponse(Guid id, string username, string name, string role, string status, Guid brandId) : base(id, username, name, role, status)
	{
		BrandId = brandId;
	}
}

public class StoreAccountLoginResponse : LoginResponse
{
	public Guid StoreId { get; set; }

	public StoreAccountLoginResponse(Guid id, string username, string name, string role, string status, Guid storeId) : base(id, username, name, role, status)
	{
		StoreId = storeId;
	}
}