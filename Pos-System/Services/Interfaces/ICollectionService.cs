using Pos_System.API.Payload.Response.Collections;

namespace Pos_System.API.Services.Interfaces
{
    public interface ICollectionService
    {
        Task<GetCollectionDetailResponse> getCollectionById(Guid collectionId, string? productName, string? productCode, int page, int size);
    }
}
