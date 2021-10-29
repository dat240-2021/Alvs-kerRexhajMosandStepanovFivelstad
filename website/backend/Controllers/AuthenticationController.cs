
using System;
using Domain.Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly AuthenticationService _authServ;

        public LoginController(ILogger<LoginController> logger, AuthenticationService authServ)
        {
            _logger = logger;
            _authServ = authServ;
        }

        [HttpPost]
        public bool Post(string uname, string password){
            Console.WriteLine(uname);
            Console.WriteLine(password);

            return true;
        }
    }


    [ApiController]
    // [Route("[controller]")]
    [Route("/api/register")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly AuthenticationService _authServ;

        public RegistrationController(ILogger<RegistrationController> logger,AuthenticationService authServ)
        {
            _logger = logger;
            _authServ = authServ;

        }

        [HttpPost]
        public string Post(string uname, string password){
            Console.WriteLine(uname);
            Console.WriteLine(password);
            return _authServ.RegisterUser(uname,password).Result;
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
