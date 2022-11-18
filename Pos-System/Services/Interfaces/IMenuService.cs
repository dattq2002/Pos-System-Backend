using Pos_System_Backend.Domain.Models;

namespace Pos_System_Backend.Services.Interfaces;

public interface IMenuService
{
	public Task<Store> GetMenuOfStore(Guid storeId, DateTime requestDateTime);
}