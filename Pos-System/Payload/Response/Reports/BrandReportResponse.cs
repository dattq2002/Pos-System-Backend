namespace Pos_System.API.Payload.Response.Reports
{
    public class BrandReportResponse
    {
        public int? TotalStoreInBrand { get; set; }
        public int? TotalAccountInBrand { get; set; }
        public int? TotalProductInBrand { get; set; }
        public int? TotalPaymentMethodInBrand { get; set; }

        public BrandReportResponse(int? totalStoreInBrand, int? totalAccountInBrand, int? totalProductInBrand, int? totalPaymentMethodInBrand)
        {
            TotalStoreInBrand = totalStoreInBrand;
            TotalAccountInBrand = totalAccountInBrand;
            TotalProductInBrand = totalProductInBrand;
            TotalPaymentMethodInBrand = totalPaymentMethodInBrand;
        }
    }
}
