using Microsoft.EntityFrameworkCore;
using NegotiationsAPI.Core.Entities;
using NegotiationsAPI.Core.Interfaces;
using NegotiationsAPI.Infrastructure.Data;
using NegotiationsAPI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationsAPI.Infrastructure.Repositories
{
    internal class NegotiationsRepository : INegotiationsRepository
    {
        private readonly ApplicationDbContext _context;

        public NegotiationsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Negotiation>> GetAllAsync()
        {
            return await _context.Negotiations.ToListAsync();
        }

        public async Task<Negotiation> GetByIdAsync(Guid id)
        {
            return await _context.Negotiations.FindAsync(id);
        }

        public async Task<Negotiation> FindActiveByUserAndProductAsync(Guid userId, Guid productId)
        {
            return await _context.Negotiations
                .FirstOrDefaultAsync(n => n.UserId == userId &&
            n.ProductId == productId &&
            (n.Status == NegotiationStatus.Pending
             || n.Status == NegotiationStatus.Rejected));
        }

        public async Task AddAsync(Negotiation negotiation)
        {
            _context.Negotiations.Add(negotiation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Negotiation negotiation)
        {
            _context.Negotiations.Update(negotiation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var negotiation = await _context.Negotiations.FindAsync(id);
            if (negotiation != null)
            {
                _context.Negotiations.Remove(negotiation);
                await _context.SaveChangesAsync();
            }
        }
    }
}
