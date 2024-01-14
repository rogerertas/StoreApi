using Xunit;
using Moq;
using StoreApi.Repositories;
using StoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace StoreApi.Tests
{
    public class StoreProductTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly ProductsController _controller;

        public StoreProductTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _controller = new ProductsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsCorrectProducts()
        {
            // Arrange
            var mockProducts = new List<Product> {
                new Product {Id = 1, Title = "Fjallraven - Foldsack No. 1 Backpack, Fits 15 Laptops", Price = 109.95M, Description = "Your perfect pack for everyday use and walks in the forest. Stash your laptop (up to 15 inches) in the padded sleeve, your everyday", Category = "men's clothing", Image = "https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_.jpg", Rating = new Rating { Rate = 3.9, Count = 120 } },
                new Product { Id = 2, Title = "Mens Casual Premium Slim Fit T-Shirts", Price = 22.3M, Description = "Slim-fitting style, contrast raglan long sleeve, three-button henley placket, light weight & soft fabric for breathable and comfortable wearing. And Solid stitched shirts with round neck made for durability and a great fit for casual fashion wear and diehard baseball fans. The Henley style round neckline includes a three-button placket.", Category = "men's clothing", Image = "https://fakestoreapi.com/img/71-3HjGNDUL._AC_SY879._SX._UX._SY._UY_.jpg", Rating = new Rating { Rate = 4.1, Count = 259 } },
                new Product { Id = 3, Title = "Mens Cotton Jacket", Price = 55.99M, Description = "Great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions, such as working, hiking, camping, mountain/rock climbing, cycling, traveling or other outdoors. Good gift choice for you or your family member. A warm hearted love to Father, husband or son in this thanksgiving or Christmas Day.", Category = "men's clothing", Image = "https://fakestoreapi.com/img/71li-ujtlUL._AC_UX679_.jpg", Rating = new Rating { Rate = 4.7, Count = 500 } } };
            _mockRepo.Setup(repo => repo.GetProductsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<string>()))
                .ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(mockProducts, okResult.Value);
        }

        [Fact]
        public async Task GetProductDetails_ReturnsCorrectProduct()
        {
            // Arrange
            var testProductId = 1;
            var testProduct = new Product
            {
                Id = testProductId,
                Title = "Fjallraven - Foldsack No. 1 Backpack, Fits 15 Laptops",
                Price = 109.95M,
                Description = "Your perfect pack for everyday use and walks in the forest. Stash your laptop (up to 15 inches) in the padded sleeve, your everyday",
                Category = "men's clothing",
                Image = "https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_.jpg",
                Rating = new Rating { Rate = 3.9, Count = 120 }
            };
            _mockRepo.Setup(repo => repo.GetProductByIdAsync(testProductId)).ReturnsAsync(testProduct);

            // Act
            var result = await _controller.GetProductDetails(testProductId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(testProduct, okResult.Value);
        }
    }
}
