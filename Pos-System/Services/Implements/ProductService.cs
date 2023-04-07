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
                selector: x => new GetProductResponse(x.Id, x.Code, x.Name, x.PicUrl, x.SellingPrice, x.DiscountPrice, x.HistoricalPrice, x.Status, x.Type),
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
                predicate: x => x.Id.Equals(brandId) && x.BrandId.Equals(brandId),
                orderBy: x => x.OrderBy(x => x.Code)
            );
            return products;
        }

        public async Task<Guid> CreateNewGroupProduct(Guid brandId, CreateNewGroupProductRequest createUpdateNewGroupProductRequest)
        {
            Guid userBrandId = Guid.Parse(GetBrandIdFromJwt());
            if (userBrandId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.EmptyBrandIdMessage);
            if(!userBrandId.Equals(brandId)) throw new BadHttpRequestException(MessageConstant.GroupProduct.WrongComboInformationMessage);

            if (createUpdateNewGroupProductRequest.ComboProductId != null)
            {
                Product product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                    predicate: x => x.Id.Equals(createUpdateNewGroupProductRequest.ComboProductId)
                        && x.Type.Equals(ProductType.COMBO.GetDescriptionFromEnum())
                        && x.BrandId.Equals(userBrandId)
                    );

                if (product == null) throw new BadHttpRequestException(MessageConstant.GroupProduct.WrongComboInformationMessage);
            }

            GroupProduct groupProductToInsert = new GroupProduct()
            {
                Id = Guid.NewGuid(),
                ComboProductId = createUpdateNewGroupProductRequest.ComboProductId,
                Name = createUpdateNewGroupProductRequest.Name,
                CombinationMode = createUpdateNewGroupProductRequest.CombinationMode.GetDescriptionFromEnum(),
                Priority = createUpdateNewGroupProductRequest.Priority,
                Quantity = createUpdateNewGroupProductRequest.Quantity,
                Status = GroupProductStatus.Active.GetDescriptionFromEnum()
            };


            List<ProductInGroup> productInGroupsToInsert = new List<ProductInGroup>();
            if(createUpdateNewGroupProductRequest.ProductIds != null || createUpdateNewGroupProductRequest.ProductIds.Count > 0)
            {
                int defaultMin = 1;
                int defaultMax = 1;
                double defaultAdditionalPrice = 0;
                int defaultPriority = 0;
                int defaultQuantity = 1;
                createUpdateNewGroupProductRequest.ProductIds.ForEach(productId =>
                    productInGroupsToInsert.Add(new ProductInGroup()
                    {
                        Id = Guid.NewGuid(),
                        GroupProductId = groupProductToInsert.Id,
                        ProductId = productId,
                        Priority = defaultPriority,
                        AdditionalPrice = defaultAdditionalPrice,
                        Min = defaultMin,
                        Max = defaultMax,
                        Quantity = defaultQuantity,
                        Status = ProductInGroupStatus.Active.GetDescriptionFromEnum()
                    }));
            }

            await _unitOfWork.GetRepository<GroupProduct>().InsertAsync(groupProductToInsert);
            await _unitOfWork.GetRepository<ProductInGroup>().InsertRangeAsync(productInGroupsToInsert);
            await _unitOfWork.CommitAsync();
            return groupProductToInsert.Id;
        }
    }
}

