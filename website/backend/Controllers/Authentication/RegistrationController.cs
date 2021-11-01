
using System;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Post(UserRequestDto user){
            if (await _authServ.RegisterUser(user.Username,user.Password)){
                return Ok(new UserResponseDto{Username = user.Username});
            }

            return UnprocessableEntity();
        }
    }



}
