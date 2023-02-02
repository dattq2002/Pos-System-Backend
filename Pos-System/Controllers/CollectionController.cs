using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Payload.Response.Collections;
using Pos_System.API.Services.Interfaces;

namespace Pos_System.API.Controllers
{
    [ApiController]
    public class CollectionController : BaseController<CollectionController>
    {
        private readonly ICollectionService _collectionService;

        public CollectionController(ILogger<CollectionController> logger, ICollectionService collectionService) : base(logger)
        {
            _collectionService = collectionService;
        }

        [HttpGet(ApiEndPointConstant.Collection.CollectionEndPoint)]
        [ProducesResponseType(typeof(GetCollectionDetailResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCollectionById(Guid id, [FromQuery]string? productName, [FromQuery]string? productCode, [FromQuery]int page, [FromQuery]int size)
        {
            var collectionResponse = await _collectionService.getCollectionById(id, productName, productCode, page, size);
            return Ok(collectionResponse);
        }


    }
}
