using AutoMapper;
using Pos_System.API.Enums;
using Pos_System.API.Models.Response;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;

namespace Pos_System.API.Mappers;

public class MappingProfiles : Profile
{
	public MappingProfiles()
	{
		//CreateMapAccountToLoginResponse();
		CreateMap<Account, LoginResponse>()
			.ForMember(des => des.AccessToken, src => src.Ignore());
	}

	private void CreateMapAccountToLoginResponse()
	{
		var configuration = new MapperConfiguration(cfg =>
		{
			cfg.CreateMap<Account, LoginResponse>()
				.ForMember(des => des.Status, src => src.MapFrom(src => EnumUtil.ParseEnum<AccountStatus>(src.Status)))
				.ForMember(des => des.AccessToken, src => src.Ignore());
			cfg.AddProfile<MappingProfiles>();
		});
	}
}