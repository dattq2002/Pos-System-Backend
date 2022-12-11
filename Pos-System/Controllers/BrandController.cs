using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Brands;
using Pos_System.API.Payload.Response;
using Pos_System.API.Payload.Response.Brands;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Validators;

namespace Pos_System.API.Controllers
{
    [ApiController]
    public class BrandController : BaseController<BrandController>
    {
	    private readonly IBrandService _brandService;
        private readonly IAccountService _accountService;
        public BrandController(ILogger<BrandController> logger, IBrandService brandService, IAccountService accountService) : base(logger)
        {
            _brandService = brandService;
            _accountService = accountService;
        }

        [CustomAuthorize(RoleEnum.SysAdmin)]
        [HttpPost(ApiEndPointConstant.Brand.BrandEndpoint)]
        [ProducesResponseType(typeof(CreateNewBrandResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNewBrand(CreateNewBrandRequest createNewBrandRequest)
        {
            CreateNewBrandResponse response =  await _brandService.CreateNewBrand(createNewBrandRequest);
            if (response == null)
            {
                _logger.LogError($"Create new brand failed with {createNewBrandRequest.Name}");
                return Problem($"{MessageConstant.Brand.CreateBrandFailMessage}: {createNewBrandRequest.Name}");
            }
            _logger.LogInformation($"Create new brand successful with {createNewBrandRequest.Name}");
            return CreatedAtAction(nameof(CreateNewBrand),response);
        }

        [CustomAuthorize(RoleEnum.SysAdmin)]
        [HttpPost(ApiEndPointConstant.Brand.CreateBrandAccountEndpoint)]
        public async Task<IActionResult> CreateBrandAccount(CreateNewBrandAccountRequest createNewBrandAccountRequest)
        {
	        CreateNewBrandAccountResponse response = await _accountService.CreateNewBrandAccount(createNewBrandAccountRequest);
	        if (response == null)
	        {
                //To-do
		        return Problem();
	        }
            _logger.LogInformation($"Create brand account successfully with brand: {createNewBrandAccountRequest.BrandId}, account: {createNewBrandAccountRequest.Username}");
	        return CreatedAtAction(nameof(CreateBrandAccount),response);
        }
    }
}
