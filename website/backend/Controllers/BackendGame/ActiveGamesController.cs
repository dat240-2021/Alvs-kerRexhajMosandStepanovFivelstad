using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Pipelines;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.BackendGame
{
    [ApiController]
    [Route("/api/games")]
    public class ActiveGamesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActiveGamesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var games = await _mediator.Send(new GetWaitingGames.Request());
            return Ok(games);
        }
    }
}