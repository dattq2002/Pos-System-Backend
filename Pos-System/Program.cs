using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Pos_System.API.Constants;
using Pos_System.API.Extensions;
using Pos_System.API.Middlewares;

var logger = NLog.LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),"/nlog.config")).GetCurrentClassLogger();

try
{
	var builder = WebApplication.CreateBuilder(args);
	builder.Logging.ClearProviders();
	builder.Host.UseNLog();

	// Add services to the container.
	builder.Services.AddCors(options =>
	{
		options.AddPolicy(name: CorsConstant.PolicyName,
			policy => { policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod(); });
	});
	builder.Services.AddControllers();
	builder.Services.AddDatabase();
	builder.Services.AddUnitOfWork();
	builder.Services.AddServices();
	builder.Services.AddJwtValidation();
	builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddConfigSwagger();

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseMiddleware<ExceptionHandlingMiddleware>();

	//app.UseHttpsRedirection();
	app.UseCors(CorsConstant.PolicyName);
	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllers();

	app.Run();
}
catch (Exception exception)
{
	logger.Error(exception, "Stop program because of exception");
}
finally
{
	NLog.LogManager.Shutdown();
}
