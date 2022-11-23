using AutoMapper;
using Pos_System.API.Enums;
using Pos_System.API.Models.Request;
using Pos_System.API.Models.Response.Product;
using Pos_System.API.Services;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements;

public class ProductService : BaseService<ProductService>, IProductService
{
	public ProductService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<ProductService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
	{
	}

}