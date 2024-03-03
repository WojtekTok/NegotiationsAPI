using Microsoft.AspNetCore.Identity;
using NegotiationsAPI.Core.Enums;
using NegotiationsAPI.Core.Interfaces;

namespace NegotiationsAPI.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task EnsureRolesCreatedAsync()
        {
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public async Task AssignRoleToUserAsync(string userEmail, Roles role)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, role.ToString());
            }
        }
    }
}
