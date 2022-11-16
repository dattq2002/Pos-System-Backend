using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pos_System_Backend.Constants;
using Pos_System_Backend.Domain.Models;
using Pos_System_Backend.Repository.Implement;
using Pos_System_Backend.Repository.Interfaces;
using Pos_System_Backend.Services.Implements;
using Pos_System_Backend.Services.Interfaces;

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

	public static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddScoped<IAccountService, AccountService>();
		return services;
	}

	public static IServiceCollection AddJwtValidation(this IServiceCollection services)
	{
		IConfiguration configuration = new ConfigurationBuilder().AddEnvironmentVariables(EnvironmentVariableConstant.Prefix).Build();
		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidIssuer = configuration.GetValue<string>(JwtConstant.Issuer),
				ValidateIssuer = true,
				ValidateAudience = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey =
					new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(configuration.GetValue<string>(JwtConstant.SecretKey)))
			};
		});
		return services;
	}

	public static IServiceCollection AddConfigSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Pos System", Version = "v1"});
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
			{
				In = ParameterLocation.Header,
				Description = "Please enter a valid token",
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				BearerFormat = "JWT",
				Scheme = "Bearer"
			});
			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new string[] { }
				}
			});
		});
		return services;
	}
}