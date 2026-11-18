using ShoppingCart.API.Models.DTOs;

namespace ShoppingCart.API.Business.Interfaces
{
    public interface IOrderBusiness
    {
        Task<int> Create(CreateOrderRequest request);

        Task<IEnumerable<GetOrderResponse>> GetAllAsync(string serverUrl);
    }
}