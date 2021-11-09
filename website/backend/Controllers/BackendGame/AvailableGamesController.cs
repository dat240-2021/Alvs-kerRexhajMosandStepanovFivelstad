using System.Linq;
using System.Threading.Tasks;
using backend.Controllers.BackendGame.Dto;
using backend.Core.Domain.BackendGame.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.BackendGame
{
    [ApiController]
    [Route("/api/games")]
    public class AvailableGamesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AvailableGamesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var games = await _mediator.Send(new GetAvailableGames.Request());
            var gamesDto = games.Select(g => new GameDto(g)).ToList();
            return Ok(gamesDto);
        }
    }
}