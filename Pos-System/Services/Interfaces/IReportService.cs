using Pos_System.API.Payload.Response.Reports;

namespace Pos_System.API.Services.Interfaces
{
    public interface IReportService
    {
        //Task<StoreReportResponse> GetStoreReport(Guid storeId, DateTime? startDate, DateTime? endDate);
        Task<SystemReport> GetSystemReport();
        Task<BrandReportResponse> GetBrandReport();
    }
}
