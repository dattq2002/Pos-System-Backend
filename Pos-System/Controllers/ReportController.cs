using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Response.Products;
using Pos_System.API.Payload.Response.Reports;
using Pos_System.API.Services.Implements;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Validators;
using Pos_System.Domain.Paginate;

namespace Pos_System.API.Controllers
{

    [ApiController]
    public class ReportController : BaseController<ReportController>
    {
        private readonly IReportService _reportService;
        public ReportController(ILogger<ReportController> logger, IReportService reportService) : base(logger)
        {
            _reportService = reportService;
        }
        [CustomAuthorize(RoleEnum.SysAdmin)]
        [HttpGet(ApiEndPointConstant.Report.SystemReportEndPoint)]
        [ProducesResponseType(typeof(SystemReport), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSystemReport()
        {
            var res = await _reportService.GetSystemReport();
            return Ok(res);
        }

        [CustomAuthorize(RoleEnum.StoreManager)]
        [HttpGet(ApiEndPointConstant.Report.StoreReportEndPoint)]
        [ProducesResponseType(typeof(StoreReportResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStoreReport(Guid id, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var res = await _reportService.GetStoreReport(id, startDate, endDate);
            return Ok(res);
        }


    }
}
