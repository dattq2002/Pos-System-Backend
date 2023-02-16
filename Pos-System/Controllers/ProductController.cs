using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Products;
using Pos_System.API.Payload.Response.Products;
using Pos_System.API.Services.Implements;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Validators;

namespace Pos_System.API.Controllers
{
    [ApiController]
    public class ProductController : BaseController<ProductController>
    {
        private readonly IProductService _productService;
        public ProductController(ILogger<ProductController> logger, IProductService productService) : base(logger)
        {
            _productService = productService;
        }

        // POST api/v1/products
        [HttpPost]
        [CustomAuthorize(RoleEnum.BrandAdmin)]
        [HttpPost(ApiEndPointConstant.Product.ProductsEndPoint)]
        [ProducesResponseType(typeof(CreateNewProductResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewCollection(CreateNewProductRequest createNewProductRequest)
        {
            _logger.LogInformation($"Start to create new product with {createNewProductRequest}");
            var response = await _productService.CreateNewProduct(createNewProductRequest);
            if (response == null)
            {
                _logger.LogInformation(
                    $"Create new product failed: {createNewProductRequest.Name}, {createNewProductRequest.Code}");
                return Ok(MessageConstant.Product.CreateNewProductFailedMessage);
            }

            return Ok(response);
        }

        // GET: api/v1/products
        [CustomAuthorize(RoleEnum.BrandAdmin)]
        [HttpGet(ApiEndPointConstant.Product.ProductsEndPoint)]
        public async Task<IActionResult> GetProducts([FromQuery] string? name, [FromQuery] int page, [FromQuery] int size)
        {
            var productsResponse = await _productService.GetProducts(name, page, size);
            return Ok(productsResponse);
        }
    }
}

