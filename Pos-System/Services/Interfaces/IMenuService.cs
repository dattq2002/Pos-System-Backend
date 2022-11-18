using Pos_System.Domain.Models;

namespace Pos_System.API.Services.Interfaces;

public interface IMenuService
{
	public Task<Store> GetMenuOfStore(Guid storeId, DateTime requestDateTime);
}