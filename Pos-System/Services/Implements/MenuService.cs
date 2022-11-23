using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Pos_System.API.Enums;
using Pos_System.API.Helpers;
using Pos_System.API.Models.Response.Product;
using Pos_System.API.Services;
using Pos_System.API.Services.Interfaces;
using Pos_System.Domain.Models;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements;

public class MenuService : BaseService<MenuService>, IMenuService
{
	public MenuService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<MenuService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
	{
	}

}