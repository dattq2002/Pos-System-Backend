using Pos_System.API.Enums;

namespace Pos_System.API.Payload.Request.Orders
{
    public class ViewOrdersRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public OrderType? OrderType { get; set; }
        public OrderStatus? OrderStatus { get; set; }

    }
}
