
using System;
using Domain.Authentication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Controllers.Authentication.Models;

namespace Controllers.Authentication
{
    [ApiController]
    [Route("/api/register")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly IAuthenticationService _authServ;

        public RegistrationController(ILogger<RegistrationController> logger, IAuthenticationService authServ)
        {
            _logger = logger;
            _authServ = authServ;

        }

        [HttpPost]
        public string Post(UserReceiveDto user){
            Console.WriteLine(user.Username);
            Console.WriteLine(user.Password);
            //TODO return meaningfull error
            return _authServ.RegisterUser(user.Username,user.Password).Result;
        }
    }



}
