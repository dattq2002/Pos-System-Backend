using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Pos_System.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : BaseController<OrderController>
	{
		public OrderController(ILogger<OrderController> logger) : base(logger)
		{
		}
	}
}
