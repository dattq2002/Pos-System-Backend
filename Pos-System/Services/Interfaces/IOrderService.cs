using Pos_System.API.Payload.Request.Orders;
using Pos_System.API.Payload.Response.Orders;

namespace Pos_System.API.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<Guid> CreateNewOrder(Guid storeId, CreateNewOrderRequest createNewOrderRequest);
        public Task<Guid> UpdateOrder(Guid storeId, Guid orderId, UpdateOrderRequest updateOrderRequest);
        public Task<GetOrderDetailResponse> GetOrderDetail(Guid storeId, Guid orderId);
    }
}
