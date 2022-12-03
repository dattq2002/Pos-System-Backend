using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.Domain.Models;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Controllers
{
	[Route(ApiEndPointConstant.ApiEndpoint)]
	[ApiController]
	public class BaseController<T> : ControllerBase where T : BaseController<T>
	{
		protected ILogger<T> _logger;

		public BaseController(ILogger<T> logger)
		{
			_logger = logger;
		}
	}
}
