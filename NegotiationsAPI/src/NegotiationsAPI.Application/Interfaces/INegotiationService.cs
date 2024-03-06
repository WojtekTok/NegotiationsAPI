using NegotiationsAPI.Core.Entities;
using NegotiationsAPI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationsAPI.Application.Interfaces
{
    internal interface INegotiationService
    {
        Task<Negotiation> StartNegotiationAsync(Guid userId, Guid productId, decimal proposedPrice);
        Task<Negotiation> UpdateNegotiationByClientAsync(Guid negotiationId, decimal proposedPrice);
        Task<Negotiation> UpdateNegotiationByEmployeeAsync(Guid negotiationId, NegotiationStatus status);
    }
}
