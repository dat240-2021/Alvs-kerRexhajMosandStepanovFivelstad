using System;
using System.Threading.Tasks;
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

    [ApiController]
    // [Route("[controller]")]
    [Route("/api/login")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
		private readonly IMediator _mediator;
        private readonly IHubContext<GamesHub> _hubContext;

        public LoginController(ILogger<LoginController> logger, IMediator mediator, IHubContext<GamesHub> hubContext)
        {
            _logger = logger;
            _mediator = mediator;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserRequestDto user)
        {
            if ( (await _mediator.Send(new LoginUser.Request(user.Username,user.Password))).Success ){
                return Ok(new GenericResponseObject<UserResponseDto>{
                    Data = new UserResponseDto{Username = user.Username}
                });
            }

            return Unauthorized();
        }
    }
    }