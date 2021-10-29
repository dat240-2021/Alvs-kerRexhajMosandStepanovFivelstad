
using System;
using Domain.Authentication.Models;
using Domain.Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Http.StatusCode;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
    //website/login
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
        public UserLogin Post(UserLogin user){


            if (_authServ.LoginUser(user.Username,user.Password).Result){
                user.Password = "";
                return user;
            }

            //
            Response.StatusCode = Unauthorized().StatusCode;
            return null;

        }
    }


    [ApiController]
    // [Route("[controller]")]
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
        public string Post(UserLogin user){
            Console.WriteLine(user.Username);
            Console.WriteLine(user.Password);
            return _authServ.RegisterUser(user.Username,user.Password).Result;
        }
    }



    [Authorize]
    [ApiController]
    // [Route("[controller]")]
    [Route("/api/home")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        [HttpGet]
        public string Get(){

            return "Success";
        }
    }



}
