using Pos_System.API.Payload.Request.Menus;

namespace Pos_System.API.Services.Interfaces
{
    public interface IMenuService
    {
        public Task<Guid> CreateNewMenu(CreateNewMenuRequest createNewMenuRequest);
    }
}
