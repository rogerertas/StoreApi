using Microsoft.AspNetCore.Mvc;
using StoreApi.Repositories;

/// <summary>
/// Controller for managing products.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Gets a list of products with optional filtering.
    /// </summary>
    /// <param name="page">Page number for pagination (default is 1).</param>
    /// <param name="pageSize">Number of items per page (default is 8).</param>
    /// <param name="minPrice">Minimum price filter (optional).</param>
    /// <param name="maxPrice">Maximum price filter (optional).</param>
    /// <param name="category">Product category filter (optional).</param>
    /// <returns>A list of products matching the criteria.</returns>
    /// <response code="200">Returns the list of products.</response>
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 8,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] string? category = null)
    {
        var products = await _productRepository.GetProductsAsync(page, pageSize, minPrice, maxPrice, category);
        return Ok(products);
    }

    /// <summary>
    /// Gets the details of a specific product.
    /// </summary>
    /// <param name="productId">The ID of the product to retrieve.</param>
    /// <returns>Details of the specified product.</returns>
    /// <response code="200">Returns the product details if found.</response>
    /// <response code="404">If the product is not found.</response>
    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductDetails(int productId)
    {
        var product = await _productRepository.GetProductByIdAsync(productId);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
}
