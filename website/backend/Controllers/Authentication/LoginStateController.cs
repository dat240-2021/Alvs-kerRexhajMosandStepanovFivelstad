using System.Threading.Tasks;
using backend.Controllers.Authentication.Dto;
using Controllers.Generics;
using Domain.Authentication.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // using Microsoft.AspNetCore.Http.StatusCode;

namespace Controllers.Authentication
{
    [ApiController]
    [Route("/api/me")]
    public class LoginStateController : ApiBaseController
    {
        private readonly ILogger<LoginStateController> _logger;
		private readonly IMediator _mediator;
        private readonly IAuthenticationService _authenticationService;

        public LoginStateController(ILogger<LoginStateController> logger, IMediator mediator, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _mediator = mediator;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _authenticationService.GetCurrentUser();
            return Ok(new UserResponseDto(user?.UserName));
        }
    }
}