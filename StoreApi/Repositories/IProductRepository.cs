using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(int page, int pageSize, decimal? minPrice, decimal? maxPrice, string category);
        Task<Product> GetProductByIdAsync(int productId);
    }
}