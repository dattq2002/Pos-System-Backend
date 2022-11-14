using Pos_System_Backend.Constants;
using Pos_System_Backend.Domain.Models;
using Pos_System_Backend.Repository.Implement;
using Pos_System_Backend.Repository.Interfaces;

namespace Pos_System_Backend.Extensions;

using Microsoft.EntityFrameworkCore;

public static class DependencyServices
{
	public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork<PosSystemContext>, UnitOfWork<PosSystemContext>>();
		return services;
	}

	public static IServiceCollection AddDatabase(this IServiceCollection services)
	{
		IConfiguration configuration = new ConfigurationBuilder().AddEnvironmentVariables(EnvironmentVariableConstant.Prefix).Build();
		services.AddDbContext<PosSystemContext>(options => options.UseSqlServer(CreateConnectionString(configuration)));
		return services;
	}

	private static string CreateConnectionString(IConfiguration configuration)
	{
		string connectionString = $"Server={configuration.GetValue<string>(DatabaseConstant.Host)},{configuration.GetValue<string>(DatabaseConstant.Port)};User Id={configuration.GetValue<string>(DatabaseConstant.UserName)};Password={configuration.GetValue<string>(DatabaseConstant.Password)};Database={configuration.GetValue<string>(DatabaseConstant.Database)}";
		return connectionString;
	}
}