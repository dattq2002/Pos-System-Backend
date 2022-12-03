using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Pos_System.API.Controllers
{
    [ApiController]
    public class BrandController : BaseController<BrandController>
    {
        private const string ControllerName = "brands";
        public BrandController(ILogger<BrandController> logger) : base(logger)
        {
        }

        [HttpPost(ControllerName)]
        public IActionResult CreateNewBrand()
        {

            return Ok();
        }
    }
}
