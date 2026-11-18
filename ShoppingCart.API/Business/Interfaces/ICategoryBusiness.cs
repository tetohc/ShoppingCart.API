using ShoppingCart.API.Models.DTOs;

namespace ShoppingCart.API.Business.Interfaces
{
    public interface ICategoryBusiness
    {
        Task<List<GetCategoryResponse>> GetAllAsync();
    }
}
