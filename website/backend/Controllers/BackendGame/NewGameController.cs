using System;
using System.Threading.Tasks;
using backend.Controllers.BackendGame.Dto;
using backend.Core.Domain.BackendGame.Pipelines;
using Controllers.Generics;
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

        public NewGameController(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post(GameSettingsDto settings)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var gameId = await _mediator.Send(new CreateGame.Request(new GameSettings
            {
                Duration = settings.Duration,
                ImagesCount = settings.ImagesCount,
                PlayersCount = settings.PlayersCount,
            },
                new Guid(userId)
                ));
            
            return Ok(new GenericResponseObject<Guid>
            {
                Data = gameId
            });
        }
    }
}