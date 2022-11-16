using Pos_System_Backend.Domain.Models;
using Pos_System_Backend.Repository.Interfaces;

namespace Pos_System_Backend.Services
{
	public abstract class BaseService<T> where T : class
	{
		protected IUnitOfWork<PosSystemContext> _unitOfWork;
		protected ILogger<T> _logger;

		public BaseService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<T> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}
	}
}
