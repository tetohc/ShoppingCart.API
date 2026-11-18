using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Business.Interfaces;
using ShoppingCart.API.Database;
using ShoppingCart.API.Models.DTOs;
using ShoppingCart.API.Models.Entities;

namespace ShoppingCart.API.Business
{
    public class OrderBusiness : IOrderBusiness
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;
        private string _folderName;

        public OrderBusiness(AppDbContext context, IConfiguration config)
        {
            _dbContext = context;
            _config = config;
            _folderName = _config["ImageSettings:ProductsFolder"]!;
        }

        public async Task<int> Create(CreateOrderRequest request)
        {
            foreach (var product in request.Details)
            {
                var productEntity = await _dbContext.Product.FindAsync(product.ProductId);
                productEntity!.StockQuantity -= product.Quantity;
            }

            var order = new Order
            {
                TotalAmount = request.TotalAmount,
                OrderDetail = request.Details.Select(d => new OrderDetail
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };
            await _dbContext.Order.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order.OrderId;
        }

        public async Task<IEnumerable<GetOrderResponse>> GetAllAsync(string serverUrl)
        {
            var orders = await _dbContext.Order
                .AsNoTracking()
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new GetOrderResponse
                (
                    o.OrderId,
                    o.OrderDate.ToString("dd-MM-yyyy HH:mm:ss"),
                    o.TotalAmount,
                    o.OrderDetail.Select(x => new OrderDetailResponse
                    (
                        $"{serverUrl}/{_folderName}/{x.Product.Image}",
                        x.Product.Name.Trim(),
                        x.Quantity,
                        x.UnitPrice * x.Quantity
                    )).ToList()
                ))
                .ToListAsync();
            return orders;
        }
    }
}