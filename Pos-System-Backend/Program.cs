using Microsoft.EntityFrameworkCore;
using Pos_System_Backend.Extensions;
using Pos_System_Backend.Model.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<PosSystemContext>(options =>
	options.UseSqlServer(
		"Server=pos-system-dev.cmzdrlsusaac.ap-southeast-1.rds.amazonaws.com,6969;User Id=possystem;Password=3w^N&Sp775B5;Database=PosSystem"));
builder.Services.AddUnitOfWork();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
