namespace ShoppingCart.API.Models.DTOs
{
    #region Requests

    public record CreateOrderRequest(decimal TotalAmount, IEnumerable<OrderDetailRequest> Details);

    public record OrderDetailRequest(int ProductId, int Quantity, decimal UnitPrice);

    #endregion Requests

    #region Responses

    public record GetOrderResponse
    (
        int OrderId,
        string Date,
        decimal TotalAmount,
        IEnumerable<OrderDetailResponse> Details
    );

    public record OrderDetailResponse
    (
        string ImageUrl,
        string ProductName,
        int Quantity,
        decimal Total
    );

    #endregion Responses
}