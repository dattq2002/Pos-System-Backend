using Microsoft.AspNetCore.Authorization;
using Pos_System.API.Enums;

namespace Pos_System.API.Validators;

public class CustomAuthorizeAttribute : AuthorizeAttribute
{
	public CustomAuthorizeAttribute(RoleEnum roleEnum)
	{
		Roles = roleEnum.ToString().Replace(" ",string.Empty);
	}
}