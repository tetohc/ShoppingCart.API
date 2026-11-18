using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using ShoppingCart.API.Business;
using ShoppingCart.API.Business.Interfaces;
using ShoppingCart.API.Database;
using ShoppingCart.API.Models.DTOs;
using ShoppingCart.API.Models.Entities;

namespace ShoppingCart.Tests.Business
{
    public class ProductBusinessTests
    {
        private static AppDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new AppDbContext(options);
        }

        /// <summary>
        /// Verifica que el método Create guarde correctamente un producto en la base de datos y devuelva el ID del producto creado.
        /// </summary>
        /// <returns>Identificador único del producto guardado.</returns>
        [Fact]
        public async Task Create_Should_SaveProduct_AndReturnId()
        {
            // Arrange
            using var context = CreateInMemoryContext("CreateDb");
            var mockFileStorage = new Mock<IFileStorageBusiness>();
            mockFileStorage.Setup(s => s.SaveFileAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync("saved.png");

            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["ImageSettings:ProductsFolder"]).Returns("uploads/products");
            var mockEnv = new Mock<IWebHostEnvironment>();

            var sut = new ProductBusiness(context, mockFileStorage.Object, mockEnv.Object, mockConfig.Object);

            var formFile = new Mock<IFormFile>().Object;
            var request = new CreateProductRequest(1, " Test Product ", 9.99m, 5, formFile);

            // Act
            var productId = await sut.Create(request);

            // Assert
            productId.Should().BeGreaterThan(0);

            var saved = await context.Product.FindAsync(productId);
            saved.Should().NotBeNull();
            saved!.Name.Should().Be("Test Product");
            saved.Image.Should().Be("saved.png");
            saved.Price.Should().Be(9.99m);
            saved.StockQuantity.Should().Be(5);
        }

        /// <summary>
        /// Verifica que GetAllAsync devuelva los productos con la URL de la imagen correctamente formateada.
        /// </summary>
        /// <returns>Lista de productos con sus datos y la URL completa de la imagen.</returns>
        [Fact]
        public async Task GetAllAsync_Should_ReturnProducts_With_ImageUrl()
        {
            // Arrange
            using var context = CreateInMemoryContext("GetAllDb");

            var category = new Category { CategoryId = 1, Name = " Category 1 ", Icon = "ic" };
            var product = new Product
            {
                ProductId = 1,
                CategoryId = 1,
                Name = " Product 1 ",
                Price = 5.5m,
                StockQuantity = 2,
                Image = "img.png",
                Category = category
            };

            context.Category.Add(category);
            context.Product.Add(product);
            await context.SaveChangesAsync();

            var mockFileStorage = new Mock<IFileStorageBusiness>();
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["ImageSettings:ProductsFolder"]).Returns("uploads/products");
            var mockEnv = new Mock<IWebHostEnvironment>();

            var sut = new ProductBusiness(context, mockFileStorage.Object, mockEnv.Object, mockConfig.Object);

            // Act
            var result = await sut.GetAllAsync("http://localhost", "0");

            // Assert
            result.Should().HaveCount(1);
            var p = result[0];
            p.ProductId.Should().Be(product.ProductId);
            p.ProductName.Should().Be(product.Name.Trim());
            p.CategoryId.Should().Be(product.CategoryId.ToString());
            p.CategoryName.Should().Be(category.Name.Trim());
            p.Price.Should().Be(product.Price);
            p.StockQuantity.Should().Be(product.StockQuantity);
            p.ImageUrl.Should().Be($"http://localhost/uploads/products/{product.Image}");
        }
    }
}