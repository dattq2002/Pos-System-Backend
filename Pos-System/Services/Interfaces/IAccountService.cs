﻿using Pos_System.API.Models.Request;
using Pos_System.API.Models.Response;
using Pos_System.Domain.Models;

namespace Pos_System.API.Services.Interfaces
{
	public interface IAccountService
	{
		Task<LoginResponse> Login(LoginRequest loginRequest);
	}
}
