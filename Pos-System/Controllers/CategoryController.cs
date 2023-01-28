using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Categories;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Validators;

namespace Pos_System.API.Controllers
{
	[ApiController]
	public class CategoryController : BaseController<CategoryController>
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService) : base(logger)
		{
			_categoryService = categoryService;
		}

		[CustomAuthorize(RoleEnum.BrandAdmin)]
		[HttpPost(ApiEndPointConstant.Category.CategoriesEndpoint)]
		[ProducesResponseType(typeof(CreateNewCategoryRequest),StatusCodes.Status200OK)]
		public async Task<IActionResult> CreateNewCategory(CreateNewCategoryRequest request)
		{
			bool isSuccessful = await _categoryService.CreateNewCategoryRequest(request);
			if (!isSuccessful) return Ok(MessageConstant.Category.CreateNewCategoryFailedMessage);
			return Ok(request);
		}
	}
}
