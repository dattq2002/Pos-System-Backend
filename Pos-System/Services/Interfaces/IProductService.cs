using Pos_System.Domain.Models;

namespace Pos_System.API.Services.Interfaces;

public interface IProductService
{
	Task<ICollection<Product>> GetProducts();
}