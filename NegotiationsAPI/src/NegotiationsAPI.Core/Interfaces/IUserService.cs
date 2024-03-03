using Microsoft.AspNetCore.Identity;

namespace NegotiationsAPI.Core.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(string email, string password);
        Task<string> LoginUserAsync(string email, string password);
    }
}
