namespace ShoppingCart.API.Models.DTOs
{
    #region Requests

    public record CreateProductRequest(int CategoryId, string Name, decimal Price,
        int StockQuantity, IFormFile Image);

    public record ReturnProductRequest(string CategoryId);

    #endregion Requests

    #region Responses

    public record GetProductResponse
    (
        int ProductId,
        string ProductName,
        string CategoryId,
        string CategoryName,
        string Name,
        decimal Price,
        int StockQuantity,
        string ImageUrl
    );

    #endregion Responses
}