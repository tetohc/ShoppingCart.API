namespace ShoppingCart.API.Business.Interfaces
{
    public interface IFileStorageBusiness
    {
        Task<string> SaveFileAsync(IFormFile file);
    }
}