using System;
using System.Threading.Tasks;
using backend.Controllers.BackendGame.Dto;
using backend.Core.Domain.BackendGame.Pipelines;
using Domain.Authentication;
using Domain.Authentication.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.BackendGame
{
    [ApiController]
    [Route("/api/game")]
    public class NewGameController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationService _authenticationService;

        public NewGameController(IMediator _mediator, UserManager<User> userManager)
        {
            this._mediator = _mediator;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post(GameSettingsDto settings)
        {
            var id = _userManager.GetUserId(HttpContext.User);

            await _mediator.Send(new CreateGame.Request(new GameSettings
            {
                Duration = settings.Duration,
                ImagesCount = settings.ImagesCount,
                PlayersCount = settings.PlayersCount,
            },
                new Guid(id)
                ));
            
            return Ok();
        }
    }
}