using NegotiationsAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationsAPI.Core.Interfaces
{
    public interface INegotiationsRepository
    {
        Task<IEnumerable<Negotiation>> GetAllAsync();
        Task<Negotiation> GetByIdAsync(Guid id);
        Task AddAsync(Negotiation negotiation);
        Task UpdateAsync(Negotiation negotiation);
        Task DeleteAsync(Guid id);
        Task<Negotiation> FindActiveByUserAndProductAsync(Guid userId, Guid productId);
    }
}
