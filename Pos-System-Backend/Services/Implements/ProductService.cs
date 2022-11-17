using AutoMapper;
using Pos_System_Backend.Domain.Models;
using Pos_System_Backend.Repository.Interfaces;
using Pos_System_Backend.Services.Interfaces;

namespace Pos_System_Backend.Services.Implements;

public class ProductService : BaseService<ProductService>,IProductService
{
	public ProductService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<ProductService> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
	{
	}

	public async Task<ICollection<Product>> GetProducts()
	{
		var products = await _unitOfWork.GetRepository<Product>().GetListAsync();
		return products;
	}
}