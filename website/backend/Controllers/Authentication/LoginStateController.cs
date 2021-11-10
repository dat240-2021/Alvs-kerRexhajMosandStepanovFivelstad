using System;
using System.Threading.Tasks;
using backend.Controllers.Authentication.Dto;
using Controllers.Authentication.Dto;
using Controllers.Generics;
using Domain.Authentication.Pipelines;
using Domain.Authentication.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Http.StatusCode;
using Microsoft.Extensions.Logging;

namespace Controllers.Authentication
{
    [Authorize]
    [ApiController]
    [Route("/api/me")]
    public class LoginStateController : ApiBaseController
    {
        private readonly ILogger<LoginStateController> _logger;
		private readonly IMediator _mediator;

        public LoginStateController(ILogger<LoginStateController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        { 
            return Ok(new UserResponseDto(User.Identity.Name));
        }
    }
}