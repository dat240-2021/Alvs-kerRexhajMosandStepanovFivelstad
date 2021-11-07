using System;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Pipelines;
using Domain.Authentication;
using Domain.Authentication.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.BackendGame
{
    [ApiController]
    [Route("/api/games/{id}/join")]
    public class JoinGameController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationService _authenticationService;

        public JoinGameController(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            await _mediator.Send(new JoinGame.Request(new Guid(userId), id));
            return Ok();
        }
    }
}