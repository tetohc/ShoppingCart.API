using ShoppingCart.API.Models.DTOs;

namespace ShoppingCart.API.Business.Interfaces
{
    public interface IProductBusiness
    {
        Task<int> Create(CreateProductRequest request);
        Task<List<GetProductResponse>> GetAllAsync(string serverUrl, string categoryId);
    }
}
