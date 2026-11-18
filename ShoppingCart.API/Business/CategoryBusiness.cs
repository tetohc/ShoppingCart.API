using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Business.Interfaces;
using ShoppingCart.API.Database;
using ShoppingCart.API.Models.DTOs;

namespace ShoppingCart.API.Business
{
    public class CategoryBusiness(AppDbContext _dbContext) : ICategoryBusiness
    {
        public async Task<List<GetCategoryResponse>> GetAllAsync()
        {
            var categories = await _dbContext.Category
                .AsNoTracking()
                .Select(c => new GetCategoryResponse
                (
                    c.CategoryId.ToString(),
                    c.Name.Trim(),
                    c.Icon.Trim()
                ))
                .ToListAsync();
            return categories;
        }
    }
}