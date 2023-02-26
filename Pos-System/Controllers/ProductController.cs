using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Products;
using Pos_System.API.Payload.Response.Categories;
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

        [CustomAuthorize(RoleEnum.BrandAdmin)]
        [HttpPost(ApiEndPointConstant.Product.ProductsEndPoint)]
        [ProducesResponseType(typeof(CreateNewProductResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewProduct(CreateNewProductRequest createNewProductRequest)
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

        [CustomAuthorize(RoleEnum.BrandAdmin)]
        [HttpGet(ApiEndPointConstant.Product.ProductsEndPoint)]
        public async Task<IActionResult> GetProducts([FromQuery] string? name, [FromQuery] ProductType? type, [FromQuery] int page, [FromQuery] int size)
        {
            var productsResponse = await _productService.GetProducts(name, type, page, size);
            return Ok(productsResponse);
        }

        [CustomAuthorize(RoleEnum.BrandAdmin)]
        [HttpGet(ApiEndPointConstant.Product.ProductEndPoint)]
        [ProducesResponseType(typeof(GetProductDetailsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            _logger.LogInformation($"Get Category by Id: {id}");
            var response = await _productService.GetProductById(id);
            return Ok(response);
        }

        [CustomAuthorize(RoleEnum.BrandAdmin)]
        [HttpPut(ApiEndPointConstant.Product.ProductEndPoint)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProductInformation(Guid id, UpdateProductRequest updateProductRequest)
        {
            _logger.LogInformation($"Start to update product with product id: {id}");
            Guid response = await _productService.UpdateProduct(id , updateProductRequest);
            if (response == Guid.Empty)
            {
                _logger.LogInformation(
                    $"Update product failed: {updateProductRequest.Name}, {updateProductRequest.Code}");
                return Ok(MessageConstant.Product.UpdateProductFailedMessage);
            }
            return Ok(response);
        }

        [CustomAuthorize(RoleEnum.BrandAdmin)]
        [HttpGet(ApiEndPointConstant.Product.ProductsInBrandEndPoint)]
        public async Task<IActionResult> GetProductsInBrand(Guid id){
            var response = await _productService.GetProductsInBrand(id);
            return Ok(response);
        }
    }
}

