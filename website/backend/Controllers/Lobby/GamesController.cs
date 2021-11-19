using System;
using System.Linq;
using System.Threading.Tasks;
using backend.Controllers.Lobby.Dto;
using backend.Core.Domain.Lobby.Models;
using backend.Core.Domain.Lobby.Pipelines;
using Controllers.Generics;
using Domain.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Lobby
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
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var game = await _mediator.Send(new CreateGame.Request(new GameSettings
                {
                    Duration = settings.RoundDuration,
                    ImagesCount = settings.ImagesCount,
                    GuessersCount = settings.GuessersCount,
                    CategoryIds = settings.CategoryIds,
                    ProposerType = settings.ProposerType
                },
                user
            ));
            
            return Ok(new GameDto(game));
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
            var user = await _userManager.GetUserAsync(HttpContext.User);
            await _mediator.Send(new JoinGame.Request(user, id, SlotRole.Guesser));
            return Ok();
        }
        
        [HttpPost("{id:guid}/leave")]
        public async Task<IActionResult> Leave(Guid id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            await _mediator.Send(new LeaveGame.Request(new Guid(userId), id));
            return Ok();
        }
        
        [HttpPost("{id:guid}/start")]
        public async Task<IActionResult> Start(Guid id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var res = await _mediator.Send(new StartGame.Request(id, Guid.Parse(userId)));
            return res.Success ? Ok() : UnprocessableEntity();
        }

    }
}