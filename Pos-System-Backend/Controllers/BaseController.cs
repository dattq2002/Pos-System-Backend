using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System_Backend.Domain.Models;
using Pos_System_Backend.Repository.Interfaces;

namespace Pos_System_Backend.Controllers
{
	[Route("api/v1")]
	[ApiController]
	public class BaseController<T> : ControllerBase where T : BaseController<T>
	{
		protected ILogger<T> _logger;
		protected IUnitOfWork<PosSystemContext> _unitOfWork;

		public BaseController(ILogger<T> logger, IUnitOfWork<PosSystemContext> unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}
	}
}
