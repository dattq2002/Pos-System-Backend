using Pos_System.API.Payload.Request.Menus;
using Pos_System.API.Payload.Response.Menus;

namespace Pos_System.API.Services.Interfaces
{
    public interface IMenuService
    {
        public Task<Guid> CreateNewMenu(CreateNewMenuRequest createNewMenuRequest);

        public Task<HasBaseMenuResponse> CheckHasBaseMenuInBrand(Guid brandId);
    }
}
