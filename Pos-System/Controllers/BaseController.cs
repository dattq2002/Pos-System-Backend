﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.Domain.Models;
using Pos_System.Repository.Interfaces;

namespace Pos_System.API.Controllers
{
	[Route("api/v1")]
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
