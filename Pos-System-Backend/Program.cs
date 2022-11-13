using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Pos_System_Backend.Constants;
using Pos_System_Backend.Extensions;
using Pos_System_Backend.Middlewares;
using Pos_System_Backend.Model.Models;

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
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseMiddleware<ExceptionHandlingMiddleware>();

	//app.UseHttpsRedirection();
	app.UseCors(CorsConstant.PolicyName);
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
