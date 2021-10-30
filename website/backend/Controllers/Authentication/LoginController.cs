using System;
using Controllers.Authentication.Models;
using Domain.Authentication.Services;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Http.StatusCode;
using Microsoft.Extensions.Logging;

namespace Controllers.Authentication
{

    [ApiController]
    // [Route("[controller]")]
    [Route("/api/login")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IAuthenticationService _authServ;

        public LoginController(ILogger<LoginController> logger, IAuthenticationService authServ)
        {
            _logger = logger;
            _authServ = authServ;
        }

        [HttpPost]
        public UserSendDto Post(UserReceiveDto user){

            if (_authServ.LoginUser(user.Username,user.Password).Result){

                return new UserSendDto{Username = user.Username};
            }

            Response.StatusCode = Unauthorized().StatusCode;
            return null;

        }
    }
    }