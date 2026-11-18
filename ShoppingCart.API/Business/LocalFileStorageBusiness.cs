using ShoppingCart.API.Business.Interfaces;

namespace ShoppingCart.API.Business
{
    public class LocalFileStorageBusiness : IFileStorageBusiness
    {
        private IWebHostEnvironment _env;
        private IConfiguration _config;
        private string _uploadFolderPath;
        private string _folderName;

        public LocalFileStorageBusiness(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
            _folderName = _config["ImageSettings:ProductsFolder"]!;
            _uploadFolderPath = Path.Combine(_env.WebRootPath, _folderName);
            Directory.CreateDirectory(_uploadFolderPath);
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            string fileName = $"{Guid.NewGuid()}{extension}";
            string filePath = Path.Combine(_uploadFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
    }
}