using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Collections;
using Pos_System.API.Payload.Response.Collections;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Validators;

namespace Pos_System.API.Controllers
{
    [ApiController]
    public class CollectionController : BaseController<CollectionController>
    {
        private readonly ICollectionService _collectionService;

        public CollectionController(ILogger<CollectionController> logger, ICollectionService collectionService) :
	        base(logger)
        {
	        _collectionService = collectionService;
        }


        [CustomAuthorize(RoleEnum.BrandAdmin)]
        [HttpGet(ApiEndPointConstant.Collection.CollectionsEndPoint)]
        public async Task<IActionResult> GetCollections([FromQuery] string? name, [FromQuery] int page, [FromQuery] int size)
        {
	        var collectionsResponse = await _collectionService.GetCollections(name, page, size);
	        return Ok(collectionsResponse);
        }

        [CustomAuthorize(RoleEnum.BrandAdmin)]
        [HttpGet(ApiEndPointConstant.Collection.CollectionEndPoint)]
        [ProducesResponseType(typeof(GetCollectionDetailResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCollectionById(Guid id, [FromQuery]string? productName, [FromQuery]string? productCode, [FromQuery]int page, [FromQuery]int size)
        {
            var collectionResponse = await _collectionService.GetCollectionById(id, productName, productCode, page, size);
            return Ok(collectionResponse);
        }

        [CustomAuthorize(RoleEnum.BrandAdmin)]
        [HttpPut(ApiEndPointConstant.Collection.CollectionEndPoint)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCollectionInformation(Guid id, UpdateCollectionInformationRequest collectionInformationRequest)
        {
            await _collectionService.UpdateCollectionInformation(id, collectionInformationRequest);
            return Ok(id);
        }

        [CustomAuthorize(RoleEnum.BrandAdmin)]
        [HttpPost(ApiEndPointConstant.Collection.CollectionsEndPoint)]
        [ProducesResponseType(typeof(CreateNewCollectionResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewCollection(CreateNewCollectionRequest createNewCollectionRequest)
        {
	        _logger.LogInformation($"Start to create new collection with {createNewCollectionRequest}");
	        var response = await _collectionService.CreateNewCollection(createNewCollectionRequest);
	        if (response == null)
	        {
		        _logger.LogInformation(
			        $"Create new collection failed: {createNewCollectionRequest.Name}, {createNewCollectionRequest.Code}");
		        return Ok(MessageConstant.Collection.CreateNewCollectionFailedMessage);
	        }

	        return Ok(response);
        }
		
    }
}
