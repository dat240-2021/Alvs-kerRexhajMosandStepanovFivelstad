using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;

namespace Authentication.Services
{
    public interface IAuthenticationService
    {
        Task<bool> ValidateUser(string username,string password);

    }


    public class AuthenticationService : IAuthenticationService {
        GameContext Db;

        public AuthenticationService(GameContext db){
            Db = db;
        }


        public async Task<bool> ValidateUser(string username,string password){

            return true;
        }

    }


}
