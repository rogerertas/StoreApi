using StoreApi.Models;
using System.Text.Json;

namespace StoreApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductRepository> _logger;
        private readonly string _baseApiUrl = "https://fakestoreapi.com/products";

        public ProductRepository(HttpClient httpClient, ILogger<ProductRepository> logger, IConfiguration configuration
)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseApiUrl = configuration["FakeStoreApi:BaseUrl"];
        }
        public async Task<IEnumerable<Product>> GetProductsAsync(int page, int pageSize, decimal? minPrice, decimal? maxPrice, string category)
        {
            if (minPrice < 0 || maxPrice < 0 || (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice))
            {
                _logger.LogError("Invalid filter parameters.");
                throw new ArgumentException("Invalid filter parameters.");
            }

            var response = await _httpClient.GetAsync(_baseApiUrl);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error fetching data from external API with status code {StatusCode}", response.StatusCode);
                throw new HttpRequestException($"Error fetching data from external API. Status code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<Product>>(content) ?? new List<Product>();

            products = FilterProducts(products, minPrice, maxPrice, category);

            var paginatedProducts = PaginateProducts(products, page, pageSize);
            return paginatedProducts;
        }

        private List<Product> FilterProducts(List<Product> products, decimal? minPrice, decimal? maxPrice, string category)
        {
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value).ToList();
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                products = products.Where(p => p.Category?.Equals(category, StringComparison.OrdinalIgnoreCase) ?? false).ToList();
            }

            return products;
        }

        private IEnumerable<Product> PaginateProducts(IEnumerable<Product> products, int page, int pageSize)
        {
            return products.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/{productId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error fetching product with ID {ProductId} from external API with status code {StatusCode}", productId, response.StatusCode);
                throw new HttpRequestException($"Error fetching product with ID {productId}. Status code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogError("No content received for product ID {productId}", productId);
                throw new Exceptions.EmptyResponseException($"No content received for product ID {productId}.");
            }

            if (!IsValidJson(content))
            {
                _logger.LogError("Invalid JSON response for product ID", productId);
                throw new Exceptions.InvalidJsonResponseException($"Invalid JSON response for product ID {productId}.");
            }

            var product = JsonSerializer.Deserialize<Product>(content);
            if (product == null)
            {
                _logger.LogInformation("Product with ID {ProductId} not found.", productId);
            }
            return product;
        }

        private bool IsValidJson(string content)
        {
            try
            {
                JsonDocument.Parse(content);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}