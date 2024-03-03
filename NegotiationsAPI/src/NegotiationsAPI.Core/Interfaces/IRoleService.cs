using NegotiationsAPI.Core.Enums;

namespace NegotiationsAPI.Core.Interfaces
{
    public interface IRoleService
    {
        Task EnsureRolesCreatedAsync();
        Task AssignRoleToUserAsync(string userEmail, Roles role);
    }
}
