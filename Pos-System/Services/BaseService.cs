﻿using AutoMapper;
using Pos_System_Backend.Domain.Models;
using Pos_System_Backend.Repository.Interfaces;

namespace Pos_System_Backend.Services
{
	public abstract class BaseService<T> where T : class
	{
		protected IUnitOfWork<PosSystemContext> _unitOfWork;
		protected ILogger<T> _logger;
		protected IMapper _mapper;
		protected IHttpContextAccessor _httpContextAccessor;
		public BaseService(IUnitOfWork<PosSystemContext> unitOfWork, ILogger<T> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
		}
	}
}
