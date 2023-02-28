using System;
using AutoMapper;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Products;
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
                Size = createNewProductRequest.Size != null ? createNewProductRequest.Size.GetDescriptionFromEnum() : null,
                HistoricalPrice = createNewProductRequest.HistoricalPrice,
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

        public async Task<IPaginate<GetProductResponse>> GetProducts(string? name, ProductType? type, int page, int size)
        {
            Guid brandId = Guid.Parse(GetBrandIdFromJwt());
            name = name?.Trim();
            if (brandId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.EmptyBrandIdMessage);
            IPaginate<GetProductResponse> productsResponse = await _unitOfWork.GetRepository<Product>().GetPagingListAsync(
                selector: x => new GetProductResponse(x.Id, x.Code, x.Name, x.PicUrl, x.Status, x.Type),
                predicate: string.IsNullOrEmpty(name) && (type == null)
                    ? x => x.BrandId.Equals(brandId)
                    : ((type == null)
                    ? x => x.BrandId.Equals(brandId) && x.Name.Contains(name)
                    : (string.IsNullOrEmpty(name)
                    ? x => x.BrandId.Equals(brandId) && x.Type.Equals(type.GetDescriptionFromEnum())
                    : x => x.BrandId.Equals(brandId) && x.Name.ToLower().Contains(name) && x.Type.Equals(type.GetDescriptionFromEnum()))),
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

        public async Task<Guid> UpdateProduct(Guid productId, UpdateProductRequest updateProductRequest)
        {
            _logger.LogInformation($"Start updating product: {productId}");
            Guid brandId = Guid.Parse(GetBrandIdFromJwt());
            if (brandId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.EmptyBrandIdMessage);
            Brand brand = await _unitOfWork.GetRepository<Brand>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(brandId));
            if (brand == null) throw new BadHttpRequestException(MessageConstant.Brand.BrandNotFoundMessage);

            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(updateProductRequest.CategoryId));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);

            Product updateProduct = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(productId));
            if (updateProduct == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFoundMessage);

            updateProduct.Code = updateProductRequest.Code;
            updateProduct.Name = updateProductRequest.Name;
            updateProduct.Description = updateProductRequest.Description;
            updateProduct.PicUrl = updateProductRequest.PicUrl;
            updateProduct.CategoryId = updateProductRequest.CategoryId;
            updateProduct.Size = updateProductRequest.Size != null ? updateProductRequest.Size.GetDescriptionFromEnum() : null;
            updateProduct.HistoricalPrice = updateProductRequest.HistoricalPrice;
            updateProduct.SellingPrice = updateProductRequest.SellingPrice;
            updateProduct.DiscountPrice = (double)(updateProductRequest.DiscountPrice == null ? 0 : updateProductRequest.DiscountPrice);
            updateProduct.DisplayOrder = updateProductRequest.DisplayOrder;
            updateProduct.Type = updateProductRequest.Type.GetDescriptionFromEnum();
            updateProduct.ParentProductId = updateProductRequest.ParentProductId;

            _unitOfWork.GetRepository<Product>().UpdateAsync(updateProduct);
            await _unitOfWork.CommitAsync();
            return productId;
        }

        public async Task<IEnumerable<GetProductDetailsResponse>> GetProductsInBrand(Guid brandId)
        {
            
            if(brandId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.EmptyBrandIdMessage);
            Brand brand = await _unitOfWork.GetRepository<Brand>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(brandId)
            );
            if(brand == null) throw new BadHttpRequestException(MessageConstant.Brand.BrandNotFoundMessage);
            IEnumerable<GetProductDetailsResponse> products = await _unitOfWork.GetRepository<Product>().GetListAsync(
                selector: x => new GetProductDetailsResponse(x.Id, x.Code, x.Name, x.SellingPrice, x.PicUrl, x.Status, x.HistoricalPrice, x.DiscountPrice, x.Description, x.DisplayOrder, x.Size, x.Type, x.ParentProductId, x.BrandId, x.CategoryId),
                predicate: x => x.Id.Equals(brandId) && x.BrandId.Equals(brandId)
            );
            return products;
        }
    }
}

