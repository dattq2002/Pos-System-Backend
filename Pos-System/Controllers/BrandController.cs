using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Brands;
using Pos_System.API.Payload.Response.Brands;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Validators;

namespace Pos_System.API.Controllers
{
    [ApiController]
    public class BrandController : BaseController<BrandController>
    {
        private const string ControllerName = "brands";
        private readonly IBrandService _brandService;
        public BrandController(ILogger<BrandController> logger, IBrandService brandService) : base(logger)
        {
            _brandService = brandService;
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpPost(ControllerName)]
        [ProducesResponseType(typeof(CreateNewBrandResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNewBrand(CreateNewBrandRequest createNewBrandRequest)
        {
            CreateNewBrandResponse response =  await _brandService.CreateNewBrand(createNewBrandRequest);
            if (response == null)
            {
                _logger.LogError($"Create new brand failed with {createNewBrandRequest.Name}");
                return Problem($"{MessageConstant.CreateNewBrandMessage.FailMessage}: {createNewBrandRequest.Name}");
            }
            _logger.LogInformation($"Create new brand successful with {createNewBrandRequest.Name}");
            return Ok(response);
        }
    }
}
