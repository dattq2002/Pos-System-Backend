using Microsoft.AspNetCore.Mvc;
using Pos_System_Backend.Model.Models;
using Pos_System_Backend.Repository.Interfaces;

namespace Pos_System_Backend.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private readonly IUnitOfWork<PosSystemContext> _unitOfWork;

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, IUnitOfWork<PosSystemContext> unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var list = await _unitOfWork.GetRepository<PaymentType>().GetListAsync();
			return Ok(list);
		}
	}
}