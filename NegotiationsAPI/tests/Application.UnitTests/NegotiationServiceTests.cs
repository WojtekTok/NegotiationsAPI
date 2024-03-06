using NegotiationsAPI.Core.Interfaces;
using Moq;
using NegotiationsAPI.Application.Services;
using NegotiationsAPI.Core.Entities;
using NegotiationsAPI.Core.Enums;

namespace Application.UnitTests
{
    public class NegotiationServiceTests
    {
        private readonly Mock<INegotiationsRepository> _negotiationRepositoryMock = new Mock<INegotiationsRepository>();
        private readonly Mock<IProductRepository> _productRepositoryMock = new Mock<IProductRepository>();
        private readonly NegotiationService _negotiationService;

        public NegotiationServiceTests()
        {
            _negotiationService = new NegotiationService(_negotiationRepositoryMock.Object, _productRepositoryMock.Object);
        }

        [Fact]
        public async Task StartNegotiationAsync_NoExistingNegotiation_CreatesNewNegotiation()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var proposedPrice = 100m;
            _negotiationRepositoryMock.Setup(repo => repo.FindActiveByUserAndProductAsync(userId, productId))
                                      .ReturnsAsync((Negotiation)null);
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                                  .ReturnsAsync(new Product { Id = productId, Price = 150m });

            // Act
            var negotiation = await _negotiationService.StartNegotiationAsync(userId, productId, proposedPrice);

            // Assert
            Assert.NotNull(negotiation);
            Assert.Equal(NegotiationStatus.Pending, negotiation.Status);
            _negotiationRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Negotiation>()), Times.Once);
        }
    }
}
