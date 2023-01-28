using Pos_System.API.Payload.Request.Categories;

namespace Pos_System.API.Services.Interfaces;

public interface ICategoryService
{
	Task<bool> CreateNewCategoryRequest(CreateNewCategoryRequest request); 
}