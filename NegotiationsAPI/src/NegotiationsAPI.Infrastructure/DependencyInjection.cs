using Microsoft.Extensions.DependencyInjection;
using NegotiationsAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NegotiationsAPI.Core.Interfaces;
using NegotiationsAPI.Infrastructure.Services;

namespace NegotiationsAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            return services;
        }
    }
}
