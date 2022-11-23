using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Models.Request;
using Pos_System.API.Models.Response.Product;
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

		//[Authorize(Roles = $"{RoleConstant.Employee},{RoleConstant.BrandManager},{RoleConstant.StoreManager}")]
		//[HttpGet(ControllerName)]
		//public async Task<IActionResult> GetProducts()
		//{
		//	var result = await _productService.GetProducts();
		//	return Ok(result);
		//}

		[Authorize]
		[HttpPost(ControllerName)]
		public async Task<IActionResult> CreateNewProduct()
		{
			return Ok();
		}

		[Authorize]
		[HttpGet(ControllerName)]
		[ProducesResponseType(typeof(List<ProductInMenuResponse>),StatusCodes.Status200OK)]
		public async Task<IActionResult> GetProductInBrand([FromQuery] Guid brandId)
		{
			return Ok();
		}

		[Authorize]
		[HttpPut(ControllerName + "/{id}")]
		public async Task<IActionResult> UpdateProduct()
		{
			return Ok();
		}
	}
}
