using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using ShoppingCart.API.Business;
using System.Text;

namespace ShoppingCart.Tests.Business
{
    public class LocalFileStorageBusinessTests
    {
        private readonly Mock<IWebHostEnvironment> _mockEnv;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly LocalFileStorageBusiness _localFileStorageBusiness;

        public LocalFileStorageBusinessTests()
        {
            _mockEnv = new Mock<IWebHostEnvironment>();
            _mockConfig = new Mock<IConfiguration>();

            string tempPath = Path.GetTempPath();
            _mockEnv.Setup(env => env.WebRootPath).Returns(tempPath);
            _mockConfig.Setup(config => config["ImageSettings:ProductsFolder"]).Returns("uploads/products");
            _localFileStorageBusiness = new LocalFileStorageBusiness(_mockEnv.Object, _mockConfig.Object);
        }

        /// <summary>
        /// Verifica que SaveFileAsync cree un archivo en la carpeta indicada y devuelva el nombre del archivo generado.
        /// </summary>
        /// <returns>El nombre del archivo generado.</returns>
        [Fact]
        public async Task SaveFileAsync_ShouldCreateFile_AndReturnFileName()
        {
            var content = "image content";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var formFile = new FormFile(stream, 0, stream.Length, "file", "test.png");

            var result = await _localFileStorageBusiness.SaveFileAsync(formFile);

            result.Should().NotBeNullOrEmpty().And.EndWith(".png");
            var fullPath = Path.Combine(_mockEnv.Object.WebRootPath, "uploads/products", result);
            File.Exists(fullPath).Should().BeTrue();
        }
    }
}