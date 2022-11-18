using Pos_System.API.Models.Response.Menu;
using Pos_System.Domain.Models;

namespace Pos_System.API.Services.Interfaces;

public interface IMenuService
{
	public Task<MenuResponse> GetMenuOfStore(Guid storeId, DateTime requestDateTime);
}