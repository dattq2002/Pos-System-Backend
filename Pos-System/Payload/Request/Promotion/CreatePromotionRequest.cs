using Pos_System.API.Enums;

namespace Pos_System.API.Payload.Request.Promotion;

public class CreatePromotionRequest
{

    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
    public PromotionEnum Type { get; set; }
    public double? MaxDiscount { get; set; }
    public double? MinConditionAmount { get; set; }
    public double? DiscountAmount { get; set; }
    public double? DiscountPercent { get; set; }
    public PromotionStatus Status { get; set; }
    public int? StartTime { get; set; }
    public int? EndTime { get; set; }
    public int? DayFilter { get; set; }
}