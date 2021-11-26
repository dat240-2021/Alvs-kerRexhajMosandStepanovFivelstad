using System.Threading.Tasks;
using MediatR;

namespace Domain.Authentication.Services
{
    public interface IAuthenticationService
    {
        Task<(bool Success, string[] errors)> RegisterUser(string username, string password);
        Task<bool> LoginUser(string username, string password);
        Task<Unit> LogoutUser();
        Task<User> GetCurrentUser();
    }
}