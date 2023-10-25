﻿using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Orders;
using Pos_System.API.Payload.Response.Orders;
using Pos_System.API.Payload.Response.Promotion;
using Pos_System.Domain.Paginate;

namespace Pos_System.API.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<Guid> CreateNewOrder(Guid storeId, CreateNewOrderRequest createNewOrderRequest);
        public Task<Guid> UpdateOrder(Guid storeId, Guid orderId, PrepareOrderInfoPrequest updateOrderRequest);
        public Task<GetOrderDetailResponse> GetOrderDetail(Guid storeId, Guid orderId);
        public Task<IPaginate<ViewOrdersResponse>> GetOrdersInStore(Guid storeId, int page, int size,
            DateTime? startDate, DateTime? endDate, OrderType? orderType, OrderStatus? orderStatus);
        public Task<List<GetPromotionResponse>> GetPromotion(Guid storeId);
        //public Task<PrepareOrderInfoResponse> PrepareOrder(PrepareOrderInfoPrequest req);


    }
}
