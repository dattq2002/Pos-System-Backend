using Pos_System.API.Enums;

namespace Pos_System.API.Payload.Request.Orders
{
    public class PrepareOrderInfoPrequest
    {
        public OrderStatus Status { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
    }
}
