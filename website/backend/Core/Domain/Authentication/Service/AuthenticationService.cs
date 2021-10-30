using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Authentication.Services
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterUser(string username,string password);
        Task<bool> LoginUser(string username,string password);
        Task<Unit> LogoutUser();
    }


    public class AuthenticationService : IAuthenticationService {
        GameContext Db;

        private UserManager<User> userManager;
        // private RoleManager<IdentityRole<Guid>> roleManager;
        private SignInManager<User> signInManager;

        public AuthenticationService(GameContext db, UserManager<User> user, SignInManager<User> signIn){
            Db = db;
            userManager = user;
            // roleManager = role; //technically not needed... leaving it here for now just in case
            signInManager = signIn;

            // if (!roleManager.Roles.Any())
            // {
            //     _ = roleManager.CreateAsync(new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "User" }).Result;
            // }
        }


        public async Task<bool> RegisterUser(string username,string password){
            var user = await userManager.FindByNameAsync(username);
            if (user != null) return false;
            
            var newUser = new User { UserName = username };
            await userManager.CreateAsync(newUser, password);
            return true;
        }
        public async Task<bool> LoginUser(string username,string password){

            User user;
            Microsoft.AspNetCore.Identity.SignInResult result;
            // try {
            user = await userManager.FindByNameAsync(username);

            if (user!=null){

                result = await signInManager.PasswordSignInAsync(user, password, false, false);
                return result.Succeeded;
            }
            return false;
            // }
            // catch (Exception e) {
            //     return e;
            // }
        }


        public async Task<Unit> LogoutUser(){
            await signInManager.SignOutAsync();
            return Unit.Value;
        }

    }


}
