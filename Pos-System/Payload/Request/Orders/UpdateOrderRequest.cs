using Pos_System.API.Enums;

namespace Pos_System.API.Payload.Request.Orders
{
    public class UpdateOrderRequest
    {
        public OrderStatus Status { get; set; }
        public Guid? paymentId { get; set; }
    }
}
