using AutoMapper;
using Pos_System.API.Services;
using Pos_System.API.Services.Interfaces;
using Pos_System.Domain.Models;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements;

public class ProductService : BaseService<ProductService>, IProductService
{
	public ProductService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<ProductService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
	{
	}

	public async Task<ICollection<Product>> GetProducts()
	{
		var products = await _unitOfWork.GetRepository<Product>().GetListAsync();
		return products;
	}
}