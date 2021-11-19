using System;
using System.Threading.Tasks;
using backend.Controllers.Authentication.Dto;
using backend.Hubs;
using Controllers.Authentication.Dto;
using Controllers.Generics;
using Domain.Authentication.Pipelines;
using Domain.Authentication.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
// using Microsoft.AspNetCore.Http.StatusCode;
using Microsoft.Extensions.Logging;

namespace Controllers.Authentication
{
    
    // [Route("[controller]")]
    [Route("/api/login")]
    public class LoginController : ApiBaseController
    {
        private readonly ILogger<LoginController> _logger;
		private readonly IMediator _mediator;

        public LoginController(ILogger<LoginController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserRequestDto user)
        {
            if ( (await _mediator.Send(new LoginUser.Request(user.Username,user.Password))).Success ){
                return Ok(new UserResponseDto(user.Username));
            }

            return Unauthorized();
        }
    }
    }