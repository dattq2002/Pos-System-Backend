using System;

using Pos_System.API.Payload.Response.Products;
using Pos_System.API.Payload.Request.Products;
using Pos_System.Domain.Paginate;

namespace Pos_System.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<GetProductDetailsResponse> GetProductById(Guid productId);

        Task<CreateNewProductResponse> CreateNewProduct(CreateNewProductRequest createNewProductRequest);

        Task<Guid> UpdateProduct(Guid productId, UpdateProductRequest updateProductRequest);

        Task<IPaginate<GetProductResponse>> GetProducts(string? name, int page, int size);
    }
}

