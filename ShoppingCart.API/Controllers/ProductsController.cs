using Microsoft.AspNetCore.Mvc;
using ShoppingCart.API.Business.Interfaces;
using ShoppingCart.API.Models.DTOs;

namespace ShoppingCart.API.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProductsController(IProductBusiness _productBusiness) : ControllerBase
    {
        /// <summary>
        /// Crea un nuevo producto en el sistema.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        {
            var productId = await _productBusiness.Create(request);
            return StatusCode(StatusCodes.Status201Created, productId);
        }

        /// <summary>
        /// Obtiene todos los productos, opcionalmente filtrados por categoría.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetProductResponse>))]
        public async Task<IActionResult> GetAll([FromQuery] ReturnProductRequest request)
        {
            var serverUrl = $"{Request.Scheme}://{Request.Host}";
            var products = await _productBusiness.GetAllAsync(serverUrl, request.CategoryId);
            return Ok(products);
        }
    }
}