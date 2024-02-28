using NegotiationsAPI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationsAPI.Core.Entities
{
    public class Negotiation
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public decimal ProposedPrice { get; set; }
        public NegotiationStatus Status { get; set; }
        public int AttemptsLeft { get; set; }
    }
}
