using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Domain.Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationService(UserManager<User> user, SignInManager<User> signIn)
        {
            _userManager = user;
            _signInManager = signIn;
        }


        public async Task<(bool Success, string[] errors)> RegisterUser(string username, string password)
        {
            var result = await _userManager.CreateAsync(new User { UserName = username }, password);
            return (result.Succeeded, result.Errors.Select(err => err.Description).ToArray());
        }

        public async Task<bool> LoginUser(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return false;
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            return result.Succeeded;

        }

        public async Task<Unit> LogoutUser()
        {
            await _signInManager.SignOutAsync();
            return Unit.Value;
        }

        public async Task<User> GetCurrentUser()
        {
            var claimsPrincipal = _signInManager.Context.User;
            var userId = _userManager.GetUserId(claimsPrincipal);
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
