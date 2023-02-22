using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Categories;
using Pos_System.API.Payload.Response.Categories;
using Pos_System.Domain.Paginate;

namespace Pos_System.API.Services.Interfaces;

public interface ICategoryService
{
    Task<bool> CreateNewCategoryRequest(CreateNewCategoryRequest request);

    Task<IPaginate<GetCategoryResponse>> GetCategories(string? name, CategoryType? type, int page, int size);

    Task<GetCategoryResponse> GetCategoryById(Guid id);

    Task<bool> UpdateCategory(Guid id, UpdateCategoryRequest request);

    Task<bool> AddExtraCategoriesToNormalCategoryRequest(Guid categoryId, List<Guid> request);

    Task<IPaginate<GetCategoryResponse>> GetExtraCategoriesByCategoryId(Guid categoryId, int page, int size);
}