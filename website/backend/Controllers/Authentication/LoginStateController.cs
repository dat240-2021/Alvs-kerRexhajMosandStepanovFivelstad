using System.Threading.Tasks;
using backend.Controllers.Authentication.Dto;
using Controllers.Generics;
using Domain.Authentication.Pipelines;
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

        public LoginStateController(ILogger<LoginStateController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUser = await _mediator.Send(new GetCurrentUser.Request());
            return Ok(new UserResponseDto(currentUser?.Id, currentUser?.UserName));
        }
    }
}