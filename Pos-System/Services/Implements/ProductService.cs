using System;
using AutoMapper;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Collections;
using Pos_System.API.Payload.Request.Products;
using Pos_System.API.Payload.Response;
using Pos_System.API.Payload.Response.Categories;
using Pos_System.API.Payload.Response.Collections;
using Pos_System.API.Payload.Response.Products;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Domain.Paginate;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements
{
    public class ProductService : BaseService<ProductService>, IProductService
    {

        public ProductService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<ProductService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewProductResponse> CreateNewProduct(CreateNewProductRequest createNewProductRequest)
        {
            _logger.LogInformation($"Start create new : {createNewProductRequest}");
            Guid brandId = Guid.Parse(GetBrandIdFromJwt());
            if (brandId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.EmptyBrandIdMessage);
            Brand brand = await _unitOfWork.GetRepository<Brand>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(brandId));
            if (brand == null) throw new BadHttpRequestException(MessageConstant.Brand.BrandNotFoundMessage);
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(Guid.Parse(createNewProductRequest.CategoryId)));
            if (category == null)
            {
                throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);
            }
            Product newProduct = new Product()
            {
                Id = Guid.NewGuid(),
                Code = createNewProductRequest.Code,
                Name = createNewProductRequest.Name,
                BrandId = brandId,
                Description = createNewProductRequest?.Description,
                PicUrl = createNewProductRequest?.PicUrl,
                Status = EnumUtil.GetDescriptionFromEnum(ProductStatus.Active),
                CategoryId = Guid.Parse(createNewProductRequest.CategoryId),
                Size = createNewProductRequest?.Size,
                HistoricalPrice = createNewProductRequest.HistoricalPrice == null ? 0 : createNewProductRequest.HistoricalPrice,
                SellingPrice = createNewProductRequest.SellingPrice,
                DiscountPrice = (double)(createNewProductRequest.DiscountPrice == null ? 0 : createNewProductRequest.DiscountPrice),
                DisplayOrder = createNewProductRequest.DisplayOrder,
                Type = EnumUtil.GetDescriptionFromEnum(createNewProductRequest.Type),
                ParentProductId = createNewProductRequest.ParentProductId != null ? Guid.Parse(createNewProductRequest?.ParentProductId) : null

            };
            await _unitOfWork.GetRepository<Product>().InsertAsync(newProduct);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) return null;
            return new CreateNewProductResponse(newProduct.Id);
        }

        public async Task<IPaginate<GetProductResponse>> GetProducts(string? name, int page, int size)
        {
            Guid brandId = Guid.Parse(GetBrandIdFromJwt());
            name = name?.Trim();
            if (brandId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.EmptyBrandIdMessage);
            IPaginate<GetProductResponse> productsResponse = await _unitOfWork.GetRepository<Product>().GetPagingListAsync(
                selector: x => new GetProductResponse(x.Id, x.Code, x.Name, x.PicUrl, x.Status, x.Type),
                predicate: string.IsNullOrEmpty(name) ? x => x.BrandId.Equals(brandId) : x => x.Name.ToLower().Contains(name.ToLower()) && x.BrandId.Equals(brandId),
                page: page,
                size: size
                );
            return productsResponse;
        }
        public async Task<GetProductDetailsResponse> GetProductById(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Product.EmptyProductIdMessage);
            Guid brandId = Guid.Parse(GetBrandIdFromJwt());
            GetProductDetailsResponse productResponse = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            selector: x => new GetProductDetailsResponse(x.Id, x.Code, x.Name, x.SellingPrice, x.PicUrl, x.Status, x.HistoricalPrice, x.DiscountPrice, x.Description, x.DisplayOrder, x.Size, x.Type, x.ParentProductId, x.BrandId, x.CategoryId),
            predicate: x => x.Id.Equals(id) && x.BrandId.Equals(brandId)
            );
            if (productResponse == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFoundMessage);
            return productResponse;
        }
    }
}

