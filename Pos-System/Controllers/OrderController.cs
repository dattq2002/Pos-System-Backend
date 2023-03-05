using Microsoft.AspNetCore.Mvc;
using Pos_System.API.Constants;
using Pos_System.API.Enums;
using Pos_System.API.Payload.Request.Menus;
using Pos_System.API.Payload.Request.Orders;
using Pos_System.API.Services.Implements;
using Pos_System.API.Services.Interfaces;
using Pos_System.API.Validators;

namespace Pos_System.API.Controllers
{
    public class OrderController : BaseController<OrderController>
    {
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService) : base(logger)
        {
            _orderService = orderService;
        }

        [CustomAuthorize(RoleEnum.Staff)]
        [HttpPost(ApiEndPointConstant.Order.OrdersEndPoint)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewOrder(Guid storeId, CreateNewOrderRequest createNewOrderRequest)
        {
            Guid newOrderIdResponse = await _orderService.CreateNewOrder(storeId, createNewOrderRequest);
            if (newOrderIdResponse == Guid.Empty)
            {
                _logger.LogInformation($"Create order failed");
                return BadRequest(MessageConstant.Order.CreateOrderFailedMessage);
            }
            _logger.LogInformation($"Create order successfully");
            return Ok(newOrderIdResponse);
        }
    }
}
