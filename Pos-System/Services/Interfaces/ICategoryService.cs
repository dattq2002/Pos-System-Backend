using Pos_System.API.Payload.Request.Categories;
using Pos_System.API.Payload.Response.Categories;
using Pos_System.Domain.Paginate;

namespace Pos_System.API.Services.Interfaces;

public interface ICategoryService
{
	Task<bool> CreateNewCategoryRequest(CreateNewCategoryRequest request);

	Task<IPaginate<GetCategoryResponse>> GetCategories(string? name, int page, int size);

	Task<GetCategoryResponse> GetCategoryById(Guid id);

	Task<bool> UpdateCategory(Guid id,UpdateCategoryRequest request);
}