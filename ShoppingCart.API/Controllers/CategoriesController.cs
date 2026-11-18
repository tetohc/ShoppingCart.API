using Microsoft.AspNetCore.Mvc;
using ShoppingCart.API.Business.Interfaces;

namespace ShoppingCart.API.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CategoriesController(ICategoryBusiness _categoryBusiness) : ControllerBase
    {
        /// <summary>
        /// Obtiene todas las categorías de productos disponibles en el sistema.
        /// </summary>
        /// <returns>Una lista de categorías de productos.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryBusiness.GetAllAsync();
            return Ok(categories);
        }
    }
}