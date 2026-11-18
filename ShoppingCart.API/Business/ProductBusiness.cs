using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Business.Interfaces;
using ShoppingCart.API.Database;
using ShoppingCart.API.Models.DTOs;
using ShoppingCart.API.Models.Entities;

namespace ShoppingCart.API.Business
{
    public class ProductBusiness : IProductBusiness
    {
        private readonly AppDbContext _dbContext;
        private readonly IFileStorageBusiness _fileStorageBusiness;
        private readonly IConfiguration _config;
        private string _folderName;

        public ProductBusiness(AppDbContext context, IFileStorageBusiness fileStorageBusiness,
            IWebHostEnvironment env, IConfiguration config)
        {
            _dbContext = context;
            _fileStorageBusiness = fileStorageBusiness;
            _config = config;
            _folderName = _config["ImageSettings:ProductsFolder"]!;
        }

        public async Task<int> Create(CreateProductRequest request)
        {
            string imagePath = await _fileStorageBusiness.SaveFileAsync(request.Image);
            var product = new Product
            {
                CategoryId = request.CategoryId,
                Name = request.Name?.Trim()!,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                Image = imagePath
            };

            await _dbContext.Product.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product.ProductId;
        }

        public async Task<List<GetProductResponse>> GetAllAsync(string serverUrl, string categoryId)
        {
            var query = _dbContext.Product.AsNoTracking();
            if (categoryId != "0")
                query = query.Where(p => p.CategoryId == int.Parse(categoryId));

            var products = await query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new GetProductResponse(
                    p.ProductId,
                    p.Name.Trim(),
                    p.CategoryId.ToString(),
                    p.Category.Name.Trim(),
                    p.Name.Trim(),
                    p.Price,
                    p.StockQuantity,
                    $"{serverUrl}/{_folderName}/{p.Image}"
                ))
                .ToListAsync();

            return products;
        }
    }
}