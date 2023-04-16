namespace Pos_System.API.Payload.Response.Reports
{
    public class SystemReport
    {
        public int? TotalBrand { get; set; }
        public int? TotalStore { get; set; }
        public int? TotalAccount { get; set; }
        public int? TotalPaymentMethod { get; set; }

        public SystemReport(int? totalBrand, int? totalStore, int? totalAccount, int? totalPaymentMethod)
        {
            TotalBrand = totalBrand;
            TotalStore = totalStore;
            TotalAccount = totalAccount;
            TotalPaymentMethod = totalPaymentMethod;
        }
    }
}
