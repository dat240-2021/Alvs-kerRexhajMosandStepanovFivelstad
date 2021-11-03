using System;
using System.Threading.Tasks;
using Controllers.Authentication.Dto;
using Domain.Authentication.Pipelines;
using Domain.Authentication.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Http.StatusCode;
using Microsoft.Extensions.Logging;

namespace Controllers.Authentication
{

    [ApiController]
    // [Route("[controller]")]
    [Route("/api/logout")]
    public class LogoutController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
		private readonly IMediator _mediator;

        public LogoutController(ILogger<LoginController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserRequestDto user){
            await _mediator.Send(new LogoutUser.Request());
            return Ok();

        }
    }
    }