using Pos_System.API.Payload.Request.Collections;
using Pos_System.API.Payload.Response.Collections;

namespace Pos_System.API.Services.Interfaces
{
    public interface ICollectionService
    {
        Task<GetCollectionDetailResponse> GetCollectionById(Guid collectionId, string? productName, string? productCode, int page, int size);

        Task<CreateNewCollectionResponse> CreateNewCollection(CreateNewCollectionRequest createNewCollectionRequest);
    }
}
