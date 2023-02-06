using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Collections;
using Pos_System.API.Payload.Response.Collections;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Services.Implements
{
    public class CollectionService : BaseService<CollectionService>, ICollectionService
    {
        public CollectionService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<CollectionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {

        }
        public async Task<GetCollectionDetailResponse> GetCollectionById(Guid collectionId, string? productName, string? productCode, int page, int size)
        {
            if (collectionId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Collection.EmptyCollectionIdMessage);

            productName = productName?.Trim();
            productCode = productCode?.Trim();

            _logger.LogInformation($"Get Collection with collection: {collectionId}");
            GetCollectionDetailResponse collectionResponse = await _unitOfWork.GetRepository<Collection>().SingleOrDefaultAsync(
                selector: x => new GetCollectionDetailResponse(x.Id, x.Name, x.Code, EnumUtil.ParseEnum<CollectionStatus>(x.Status), x.PicUrl, x.Description),
                predicate: x => x.Id.Equals(collectionId));

            if(collectionResponse == null) throw new BadHttpRequestException(MessageConstant.Collection.CollectionNotFoundMessage);

            List<Guid> productIds = (List<Guid>)await _unitOfWork.GetRepository<CollectionProduct>().GetListAsync(
                selector: x => x.ProductId,
                predicate: x => x.CollectionId.Equals(collectionId) && x.Status.Equals(CollectionStatus.Active.GetDescriptionFromEnum())
                );


            collectionResponse.Products = await _unitOfWork.GetRepository<Product>().GetPagingListAsync(
                             selector: x => new ProductOfCollection(x.Id, x.Name, x.Description, x.Code, x.PicUrl, x.SellingPrice),
                             predicate: x => !string.IsNullOrWhiteSpace(productName) && !string.IsNullOrWhiteSpace(productCode) ? x.Name.ToLower().Contains(productName.ToLower())
                                                 && x.Code.ToLower().Contains(productCode.ToLower()) && productIds.Contains(x.Id) && x.Status.Equals(ProductStatus.Active.GetDescriptionFromEnum())
                                             : !string.IsNullOrWhiteSpace(productName) ? x.Name.ToLower().Contains(productName.ToLower()) && productIds.Contains(x.Id) && x.Status.Equals(ProductStatus.Active.GetDescriptionFromEnum())
                                             : !string.IsNullOrWhiteSpace(productCode) ? x.Code.ToLower().Contains(productCode.ToLower()) && productIds.Contains(x.Id) && x.Status.Equals(ProductStatus.Active.GetDescriptionFromEnum())
                                             : productIds.Contains(x.Id) && x.Status.Equals(ProductStatus.Active.GetDescriptionFromEnum()),
                             page: page,
                             size: size);

            return collectionResponse;
        }

        public async Task<CreateNewCollectionResponse> CreateNewCollection(CreateNewCollectionRequest createNewCollectionRequest)
        {
	        createNewCollectionRequest.TrimString();
	        var brandId = Guid.Parse(GetBrandIdFromJwt());
	        Collection newCollection = new Collection()
	        {
				Id = Guid.NewGuid(),
                Code = createNewCollectionRequest.Code,
                Name = createNewCollectionRequest.Name,
                BrandId = brandId,
                Description = createNewCollectionRequest?.Description,
                PicUrl = createNewCollectionRequest?.PicUrl,
                Status = CollectionStatus.Deactivate.GetDescriptionFromEnum(),

	        };
	        await _unitOfWork.GetRepository<Collection>().InsertAsync(newCollection);
	        bool isSuccessful =  await _unitOfWork.CommitAsync() > 0;
	        if (!isSuccessful) return null;
	        return new CreateNewCollectionResponse(newCollection.Id,newCollection.Name, newCollection.Code, newCollection.Status, newCollection.Description, newCollection.PicUrl, newCollection.BrandId);
        }
    }
}
