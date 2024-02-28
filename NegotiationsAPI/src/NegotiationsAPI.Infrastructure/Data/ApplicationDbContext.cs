using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NegotiationsAPI.Core.Entities;



namespace NegotiationsAPI.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Negotiation> Negotiations { get; set; }
        // Nie potrzebujesz DbSet<User>, ponieważ IdentityUser jest już zarządzany przez Identity TODO: to usunąć
    }
}
