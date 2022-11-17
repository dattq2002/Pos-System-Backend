using Pos_System_Backend.Domain.Models;

namespace Pos_System_Backend.Services.Interfaces;

public interface IProductService
{
	Task<ICollection<Product>> GetProducts();
}