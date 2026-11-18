using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.API.Business;
using ShoppingCart.API.Business.Interfaces;
using ShoppingCart.API.Models.DTOs;

namespace ShoppingCart.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController(IOrderBusiness _orderBusiness) : ControllerBase
    {
        /// <summary>
        /// Crea un nueva orden en el sistema.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var orderId = await _orderBusiness.Create(request);
            return StatusCode(StatusCodes.Status201Created, orderId);
        }

        /// <summary>
        /// Obtiene todas las órdenes en el sistema.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetOrderResponse>))]
        public async Task<IActionResult> GetAll()
        {
            var serverUrl = $"{Request.Scheme}://{Request.Host}";
            var orders = await _orderBusiness.GetAllAsync(serverUrl);
            return Ok(orders);
        }
    }
}
