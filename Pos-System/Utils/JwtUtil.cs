using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Pos_System.API.Constants;
using Pos_System.Domain.Models;

namespace Pos_System.API.Utils;

public class JwtUtil
{
	private JwtUtil()
	{

	}

	public static string GenerateJwtToken(Account account)
	{
		IConfiguration configuration = new ConfigurationBuilder()
			.AddEnvironmentVariables(EnvironmentVariableConstant.Prefix).Build();
		JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
		SymmetricSecurityKey secrectKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>(JwtConstant.SecretKey)));
		var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256Signature);
		string issuer = configuration.GetValue<string>(JwtConstant.Issuer);
		List<Claim> claims = new List<Claim>()
		{
			new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
			new Claim(JwtRegisteredClaimNames.Sub,account.Username),
			new Claim(ClaimTypes.Role,account.Role.Name),
		};
		var expires = DateTime.Now.AddMinutes(configuration.GetValue<long>(JwtConstant.TokenExpireInMinutes));
		var token = new JwtSecurityToken(issuer, null, claims, notBefore: DateTime.Now, expires, credentials);
		return jwtHandler.WriteToken(token);
	}
}