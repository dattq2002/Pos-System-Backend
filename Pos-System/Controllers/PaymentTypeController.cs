using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Response.PaymentTypes;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Validators;

namespace Pos_System.API.Controllers
{
	public class PaymentTypeController : BaseController<PaymentTypeController>
	{
		private readonly IPaymentTypeService _paymentTypeService;

		public PaymentTypeController(ILogger<PaymentTypeController> logger, IPaymentTypeService paymentTypeService) : base(logger)
		{
			_paymentTypeService = paymentTypeService;
		}

		[CustomAuthorize(RoleEnum.Staff)]
		[HttpGet(ApiEndPointConstant.PaymentType.PaymentTypesEndPoint)]
		[ProducesResponseType(typeof(GetPaymentTypeDetailResponse), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllPaymentTypes()
		{
			var response = await _paymentTypeService.GetAllPaymentTypesByBrandId();
			return Ok(response);
		}
	}
}
