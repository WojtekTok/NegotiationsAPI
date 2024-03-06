using NegotiationsAPI.Application.Interfaces;
using NegotiationsAPI.Core.Entities;
using NegotiationsAPI.Core.Interfaces;
using NegotiationsAPI.Core.Enums;

namespace NegotiationsAPI.Application.Services
{
    public class NegotiationService : INegotiationService
    {
        private readonly INegotiationsRepository _negotiationRepository;
        private readonly IProductRepository _productRepository;
        
        public NegotiationService(INegotiationsRepository negotiationRepository, IProductRepository productRepository)
        {
            _negotiationRepository = negotiationRepository;
            _productRepository = productRepository;
        }

        public async Task<Negotiation> StartNegotiationAsync(Guid userId, Guid productId, decimal proposedPrice)
        {
            var existingNegotiation = await _negotiationRepository.FindActiveByUserAndProductAsync(userId,
                                                                                                   productId);
            if (existingNegotiation != null)
            {
                throw new Exception("Negotiation already in progress.");
            }

            var product = await _productRepository.GetByIdAsync(productId);
            var negotiation = new Negotiation
            {
                ProductId = productId,
                UserId = userId,
                ProposedPrice = proposedPrice,
                Status = NegotiationStatus.Pending,
                AttemptsLeft = 3
            };

            if (proposedPrice >= product.Price)
            {
                negotiation.Status = NegotiationStatus.Accepted;
            }
            else if (proposedPrice < product.Price / 2)
            {
                negotiation.Status = NegotiationStatus.Rejected;
            }

            await _negotiationRepository.AddAsync(negotiation);
            return negotiation;
        }

        public async Task<Negotiation> UpdateNegotiationByClientAsync(Guid negotiationId, decimal newPrice)
        {
            var negotiation = await _negotiationRepository.GetByIdAsync(negotiationId);
            if (negotiation == null || negotiation.Status == NegotiationStatus.Pending || negotiation.AttemptsLeft >= 3)
            {
                throw new Exception("Negotiation cannot be updated.");
            }
            var product = await _productRepository.GetByIdAsync(negotiation.ProductId);
            negotiation.ProposedPrice = newPrice;
            negotiation.AttemptsLeft--;

            if (newPrice >= product.Price)
            {
                negotiation.Status = NegotiationStatus.Accepted;
            }
            else if (newPrice < product.Price / 2)
            {
                negotiation.Status = NegotiationStatus.Rejected;
            }

            if (negotiation.AttemptsLeft <= 0 && negotiation.Status == NegotiationStatus.Pending)
            {
                negotiation.Status = NegotiationStatus.Rejected;
            }

            await _negotiationRepository.UpdateAsync(negotiation);
            return negotiation;
        }

        public async Task<Negotiation> UpdateNegotiationByEmployeeAsync(Guid negotiationId, NegotiationStatus status)
        {
            var negotiation = await _negotiationRepository.GetByIdAsync(negotiationId);
            if (negotiation == null || negotiation.Status != NegotiationStatus.Pending)
            {
                throw new Exception("Negotiation cannot be updated.");
            }
            negotiation.Status = status;
            await _negotiationRepository.UpdateAsync(negotiation);
            return negotiation;
        }
    }
}
