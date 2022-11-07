using Pos_System_Backend.Model.Models;
using Pos_System_Backend.Repository.Implement;
using Pos_System_Backend.Repository.Interfaces;

namespace Pos_System_Backend.Extensions;

public static class DependencyServices
{
	public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork<PosSystemContext>, UnitOfWork<PosSystemContext>>();
		return services;
	}
}