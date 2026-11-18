using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Business;
using ShoppingCart.API.Database;
using ShoppingCart.API.Models.Entities;

namespace ShoppingCart.Tests.Business
{
    public class CategoryBusinessTests
    {
        private static AppDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new AppDbContext(options);
        }

        /// <summary>
        /// Verifica que GetAllAsync retorna todas las categorías correctamente.
        /// </summary>
        /// <returns>Lista de categorías.</returns>
        [Fact]
        public async Task GetAllAsync_Should_ReturnAllCategories()
        {
            using var context = CreateInMemoryContext("CategoryDb");
            context.Category.Add(new Category { Name = "Category 1", Icon = "icon1.png" });
            context.Category.Add(new Category { Name = "Category 2", Icon = "icon2.png" });
            await context.SaveChangesAsync();
            var sut = new CategoryBusiness(context);

            var categories = await sut.GetAllAsync();

            categories.Should().HaveCount(2);
            categories[0].Name.Should().Be("Category 1");
            categories[1].Name.Should().Be("Category 2");
        }

        /// <summary>
        /// Verifica que GetAllAsync devuelva una lista vacía cuando no existen categorías.
        /// </summary>
        /// <returns>Lista vacía de categorías.</returns>
        [Fact]
        public async Task GetAllAsync_Should_ReturnEmptyList_WhenNoCategoriesExist()
        {
            using var context = CreateInMemoryContext("EmptyCategoryDb");
            var sut = new CategoryBusiness(context);

            var categories = await sut.GetAllAsync();

            categories.Should().NotBeNull();
            categories.Should().BeEmpty("no hay categorías en la base de datos");
        }
    }
}