using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Services.Interfaces;

namespace Pos_System.API.Controllers
{
	[ApiController]
	public class ProductController : BaseController<ProductController>
	{
		private const string ControllerName = "products";
		private readonly IProductService _productService;
		public ProductController(ILogger<ProductController> logger, IProductService productService) : base(logger)
		{
			_productService = productService;
		}

		[Authorize(Roles = $"{RoleConstant.Employee},{RoleConstant.BrandManager},{RoleConstant.StoreManager}")]
		[HttpGet(ControllerName)]
		public async Task<IActionResult> GetProducts()
		{
			var result = await _productService.GetProducts();
			return Ok(result);
		}


	}
}
