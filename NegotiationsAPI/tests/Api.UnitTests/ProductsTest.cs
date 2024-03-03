using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NegotiationsAPI.Api.Controllers;
using NegotiationsAPI.Core.Entities;
using NegotiationsAPI.Core.Interfaces;

namespace Api.UnitTests
{
    public class ProductsTest
    {
        [Fact]
        public async Task Create_Product_ReturnsCreatedAtAction()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            var controller = new ProductsController(mockRepo.Object);
            var newProductDto = new Product { Name = "New Product", Price = 99.99m };

            // Act
            var result = await controller.Create(newProductDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once());
        }

        [Fact]
        public async Task Delete_Product_ReturnsNoContent()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            var controller = new ProductsController(mockRepo.Object);
            var productId = Guid.NewGuid();

            mockRepo.Setup(r => r.DeleteAsync(productId)).Returns(Task.CompletedTask).Verifiable();

            // Act
            var result = await controller.Delete(productId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockRepo.Verify(r => r.DeleteAsync(productId), Times.Once());
        }

        [Fact]
        public async Task GetById_Product_ReturnsProduct()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            var controller = new ProductsController(mockRepo.Object);
            var productId = Guid.NewGuid();
            var expectedProduct = new Product { Id = productId, Name = "Existing Product", Price = 50.00m };

            mockRepo.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(expectedProduct);

            // Act
            var result = await controller.GetById(productId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(actionResult.Value);
            Assert.Equal(expectedProduct.Name, returnedProduct.Name);
            Assert.Equal(expectedProduct.Price, returnedProduct.Price);
        }
    }
}
