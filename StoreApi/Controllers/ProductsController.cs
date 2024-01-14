using Microsoft.AspNetCore.Mvc;
using StoreApi.Repositories;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

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

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductDetails(int productId)
    {
        var product = await _productRepository.GetProductByIdAsync(productId);
        return Ok(product);
    }
}
