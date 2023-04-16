namespace Pos_System.API.Payload.Response.Reports
{
    public class BrandReportResponse
    {
        public double BasicRevenue { get; set; }
        public List<double>? RevenueByTimeline { get; set; }
        public int TotalOrder { get; set; }
        public double TotalDiscount { get; set; }
        public int TotalOrderUsingCash { get; set; }
        public int TotalOrderUsingOnline { get; set; }
    }
}
