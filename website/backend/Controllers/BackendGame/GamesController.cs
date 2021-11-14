using System;
using System.Linq;
using System.Threading.Tasks;
using backend.Controllers.BackendGame.Dto;
using backend.Core.Domain.BackendGame.Models;
using backend.Core.Domain.BackendGame.Pipelines;
using Controllers.Generics;
using Domain.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.BackendGame
{
    [Authorize]
    [ApiController]
    [Route("api/games")]
    public class GamesController: ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public GamesController(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(GameSettingsDto settings)
        {
            var userId = Guid.Parse(_userManager.GetUserId(HttpContext.User));

            var gameId = await _mediator.Send(new CreateGame.Request(new GameSettings
                {
                    Duration = settings.Duration,
                    ImageCount = settings.ImagesCount,
                    PlayersCount = settings.PlayersCount,
                },
                userId
            ));
            
            return Ok(gameId);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var games = await _mediator.Send(new GetAvailableGames.Request());
            var gamesDto = games.Select(g => new GameDto(g)).ToList();
            return Ok(gamesDto);
        }
        
        [HttpPost("{id:guid}/join")]
        public async Task<IActionResult> Join(Guid id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            await _mediator.Send(new JoinGame.Request(new Guid(userId), id));
            return Ok();
        }
        
        [HttpPost("{id:guid}/leave")]
        public async Task<IActionResult> Leave(Guid id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            await _mediator.Send(new LeaveGame.Request(new Guid(userId), id));
            return Ok();
        }
    }
}