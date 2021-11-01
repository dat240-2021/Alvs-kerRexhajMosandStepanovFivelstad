using System;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Post(UserRequestDto user){

            if (await _authServ.LoginUser(user.Username,user.Password)){
                return Ok(new UserResponseDto{Username = user.Username});
            }

            return Unauthorized();
        }
    }
    }