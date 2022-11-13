using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System_Backend.Model.Models;
using Pos_System_Backend.Repository.Interfaces;

namespace Pos_System_Backend.Controllers
{
	[ApiController]
	public class AccountController : BaseController<AccountController>
	{
		public AccountController(ILogger<AccountController> logger, IUnitOfWork<PosSystemContext> unitOfWork) : base(logger, unitOfWork)
		{
		}

		[HttpGet("Accounts")]
		public async Task<IActionResult> GetAccount()
		{
			_logger.LogInformation("Get Payment type");
			ICollection<PaymentType> result = await _unitOfWork.GetRepository<PaymentType>().GetListAsync();
			return Ok(result);
		}
	}
}
