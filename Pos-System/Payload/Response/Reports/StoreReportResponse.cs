namespace Pos_System.API.Payload.Response.Reports
{
    public class StoreReportResponse
    {
        public double BasicRevenue { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalFinalRevenue { get; set; }
        public List<string>? listTimeLine { get; set; }
        public List<double>? TotalRevenueByTimeline { get; set; }
        public List<int>? TotalOrderByTimeline { get; set; }

        public List<string>? listOrderStatus { get; set; }
        public List<int>? TotalOrderByStatus { get; set; }

        public List<string>? listOrderType { get; set; }
        public List<int>? TotalOrderByType { get; set; }

        public StoreReportResponse(double basicRevenue, double totalDiscount, double totalFinalRevenue, List<string>? listTimeLine, List<double>? totalRevenueByTimeline, List<int>? totalOrderByTimeline, List<string>? listOrderStatus, List<int>? totalOrderByStatus, List<string>? listOrderType, List<int>? totalOrderByType)
        {
            BasicRevenue = basicRevenue;
            TotalDiscount = totalDiscount;
            TotalFinalRevenue = totalFinalRevenue;
            this.listTimeLine = listTimeLine;
            TotalRevenueByTimeline = totalRevenueByTimeline;
            TotalOrderByTimeline = totalOrderByTimeline;
            this.listOrderStatus = listOrderStatus;
            TotalOrderByStatus = totalOrderByStatus;
            this.listOrderType = listOrderType;
            TotalOrderByType = totalOrderByType;
        }
    }
}
