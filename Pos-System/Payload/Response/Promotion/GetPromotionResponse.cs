using System;
using Pos_System.API.Enums;

namespace Pos_System.API.Payload.Response.Promotion
{
    public class GetPromotionResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public PromotionEnum Type { get; set; }
        public double? MaxDiscount { get; set; }
        public double? MinConditionAmount { get; set; }
        public double? DiscountAmount { get; set; }
        public double? DiscountPercent { get; set; }
        public string? Status { get; set; }
    }
}

