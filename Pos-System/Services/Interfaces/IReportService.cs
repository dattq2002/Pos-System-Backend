using System;
using Pos_System.API.Payload.Response.Stores;

namespace Pos_System.API.Services.Interfaces
{
	public interface IReportService
	{
        Task<GetStoreEndDayReport> GetStoreEndDayReport(Guid storeId, DateTime? startDate);
    }
}

